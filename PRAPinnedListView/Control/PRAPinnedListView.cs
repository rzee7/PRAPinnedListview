using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRAPinnedListView
{
    public class PRAListView : ListView, Android.Widget.AbsListView.IOnScrollListener
    {

        #region Private Fields

        // fields used for handling touch events
        private Rect mTouchRect = new Rect();
        private PointF mTouchPoint = new PointF();
        private int mTouchSlop;
        private View mTouchTarget;
        private MotionEvent mDownEvent;

        // fields used for drawing shadow under a pinned section
        private GradientDrawable mShadowDrawable;
        private int mSectionsDistanceY;
        private int mShadowHeight;

        PinnedSection mRecycleSection;

        PinnedSection mPinnedSection;

        int mTranslateY;

        //
        Android.Widget.AbsListView.IOnScrollListener mDelegateOnScrollListener;
        PRADataSetObserver mDataSetObserver = null;
        #endregion

        #region Constructor

        public PRAListView(Context context)
            : base(context)
        {
            InitView();
        }
        public PRAListView(IntPtr a, JniHandleOwnership b)
            : base(a, b)
        {
            InitView();
        }
        public PRAListView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            InitView();
        }

        public PRAListView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            InitView();
        }


        #endregion

        #region Init View

        private void InitView()
        {
            mDataSetObserver = new PRADataSetObserver(RecreatePinnedShadow, null);
            //
            SetOnScrollListener(this);
            mTouchSlop = ViewConfiguration.Get(Context).ScaledTouchSlop;
            InitShadow(true);
        }

        #endregion

        #region Set Shadow Visible

        public void SetShadowVisible(bool visible)
        {
            InitShadow(visible);
            if (mPinnedSection != null)
            {
                View v = mPinnedSection.ViewHolder;
                Invalidate(v.Left, v.Top, v.Right, v.Bottom + mShadowHeight);
            }
        }

        #endregion

        #region Init Shadow

        //-- pinned section drawing methods

        public void InitShadow(bool visible)
        {
            if (visible)
            {
                if (mShadowDrawable == null)
                {
                    mShadowDrawable = new GradientDrawable(GradientDrawable.Orientation.TopBottom,
                            new int[] { Color.ParseColor("#ffa0a0a0"), Color.ParseColor("#50a0a0a0"), Color.ParseColor("#00a0a0a0") });
                    mShadowHeight = (int)(8 * Resources.DisplayMetrics.Density);
                }
            }
            else
            {
                if (mShadowDrawable != null)
                {
                    mShadowDrawable = null;
                    mShadowHeight = 0;
                }
            }
        }

        #endregion

        #region Is Item View Type Pinned

        public bool IsItemViewTypePinned(IListAdapter adapter, int viewType)
        {
            if (adapter.GetType().IsAssignableFrom(typeof(HeaderViewListAdapter)))
            {
                adapter = ((HeaderViewListAdapter)adapter).WrappedAdapter;
            }
            return ((IPinnedSectionListAdapter)adapter).IsItemViewTypePinned(viewType);
        }

        #endregion

        #region Create Pinned Shadow

        /** Create shadow wrapper with a pinned view for a view at given position */
        void CreatePinnedShadow(int position)
        {

            // try to recycle shadow
            PinnedSection pinnedShadow = mRecycleSection;
            mRecycleSection = null;

            // create new shadow, if needed
            if (pinnedShadow == null) pinnedShadow = new PinnedSection();
            // request new view using recycled view, if such
            View pinnedView = Adapter.GetView(position, pinnedShadow.ViewHolder, this);

            // read layout parameters
            LayoutParams layoutParams = (LayoutParams)pinnedView.LayoutParameters;
            if (layoutParams == null)
            {
                layoutParams = (LayoutParams)GenerateDefaultLayoutParams();
                pinnedView.LayoutParameters = layoutParams;
            }

            MeasureSpecMode heightMode = MeasureSpec.GetMode(layoutParams.Height);
            int heightSize = MeasureSpec.GetSize(layoutParams.Height);

            if (heightMode == MeasureSpecMode.Unspecified) heightMode = MeasureSpecMode.Exactly;

            int maxHeight = Height - ListPaddingTop - ListPaddingBottom;
            if (heightSize > maxHeight) heightSize = maxHeight;

            // measure & layout
            int ws = MeasureSpec.MakeMeasureSpec(Width - ListPaddingLeft - ListPaddingRight, MeasureSpecMode.Exactly);
            int hs = MeasureSpec.MakeMeasureSpec(heightSize, heightMode);
            pinnedView.Measure(ws, hs);
            pinnedView.Layout(0, 0, pinnedView.MeasuredWidth, pinnedView.MeasuredHeight);
            mTranslateY = 0;

            // initialize pinned shadow
            pinnedShadow.ViewHolder = pinnedView;
            pinnedShadow.Position = position;
            pinnedShadow.ID = Adapter.GetItemId(position);

            // store pinned shadow
            mPinnedSection = pinnedShadow;
        }

        #endregion

        #region Re Create Pinned Shadow

        void RecreatePinnedShadow()
        {
            DestroyPinnedShadow();
            IListAdapter adapter = Adapter;
            if (adapter != null && adapter.Count > 0)
            {
                int firstVisiblePosition = FirstVisiblePosition;
                int sectionPosition = FindCurrentSectionPosition(firstVisiblePosition);
                if (sectionPosition == -1) return; // no views to pin, exit
                EnsureShadowForPosition(sectionPosition,
                        firstVisiblePosition, LastVisiblePosition - firstVisiblePosition);
            }
        }

        #endregion

        #region Destroy Pinned Shadow

        void DestroyPinnedShadow()
        {
            if (mPinnedSection != null)
            {
                mRecycleSection = mPinnedSection;
                mPinnedSection = null;
            }
        }

        #endregion

        #region Ensure Shadow For Pins

        void EnsureShadowForPosition(int sectionPosition, int firstVisibleItem, int visibleItemCount)
        {
            if (visibleItemCount < 2)
            { // no need for creating shadow at all, we have a single visible item
                DestroyPinnedShadow();
                return;
            }

            if (mPinnedSection != null
                    && mPinnedSection.Position != sectionPosition)
            { // invalidate shadow, if required
                DestroyPinnedShadow();
            }

            if (mPinnedSection == null)
            { // create shadow, if empty
                CreatePinnedShadow(sectionPosition);
            }

            // align shadow according to next section position, if needed
            int nextPosition = sectionPosition + 1;
            if (nextPosition < Count)
            {
                int nextSectionPosition = FindFirstVisibleSectionPosition(nextPosition,
                        visibleItemCount - (nextPosition - firstVisibleItem));
                if (nextSectionPosition > -1)
                {
                    View nextSectionView = GetChildAt(nextSectionPosition - firstVisibleItem);
                    int bottom = mPinnedSection.ViewHolder.Bottom + PaddingTop;
                    mSectionsDistanceY = nextSectionView.Top - bottom;
                    if (mSectionsDistanceY < 0)
                    {
                        // next section overlaps pinned shadow, move it up
                        mTranslateY = mSectionsDistanceY;
                    }
                    else
                    {
                        // next section does not overlap with pinned, stick to top
                        mTranslateY = 0;
                    }
                }
                else
                {
                    // no other sections are visible, stick to top
                    mTranslateY = 0;
                    mSectionsDistanceY = int.MaxValue;
                }
            }
        }

        #endregion

        #region First Visible Section Postion

        int FindFirstVisibleSectionPosition(int firstVisibleItem, int visibleItemCount)
        {
            IListAdapter adapter = Adapter;

            int adapterDataCount = adapter.Count;
            if (LastVisiblePosition >= adapterDataCount) return -1; // dataset has changed, no candidate

            if (firstVisibleItem + visibleItemCount >= adapterDataCount)
            {//added to prevent index Outofbound (in case)
                visibleItemCount = adapterDataCount - firstVisibleItem;
            }

            for (int childIndex = 0; childIndex < visibleItemCount; childIndex++)
            {
                int position = firstVisibleItem + childIndex;
                int viewType = adapter.GetItemViewType(position);
                if (IsItemViewTypePinned(adapter, viewType)) return position;
            }
            return -1;
        }

        #endregion

        #region Find Current Position

        int FindCurrentSectionPosition(int fromPosition)
        {
            IListAdapter adapter = Adapter;

            if (fromPosition >= adapter.Count) return -1; // dataset has changed, no candidate

            if (adapter.GetType().IsAssignableFrom(typeof(ISectionIndexer)))
            {
                // try fast way by asking section indexer
                ISectionIndexer indexer = (ISectionIndexer)adapter;
                int sectionPosition = indexer.GetSectionForPosition(fromPosition);
                int itemPosition = indexer.GetPositionForSection(sectionPosition);
                int typeView = adapter.GetItemViewType(itemPosition);
                if (IsItemViewTypePinned(adapter, typeView))
                {
                    return itemPosition;
                } // else, no luck
            }

            // try slow way by looking through to the next section item above
            for (int position = fromPosition; position >= 0; position--)
            {
                int viewType = adapter.GetItemViewType(position);
                if (IsItemViewTypePinned(adapter, viewType)) return position;
            }
            return -1; // no candidate found
        }

        #endregion

        #region OnScroll Method

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (mDelegateOnScrollListener != null)
            { // delegate
                mDelegateOnScrollListener.OnScroll(view, firstVisibleItem, visibleItemCount, totalItemCount);
            }

            //
            IListAdapter adapter = Adapter;
            if (adapter == null || visibleItemCount == 0) return; // nothing to do

            bool isFirstVisibleItemSection = IsItemViewTypePinned(adapter, adapter.GetItemViewType(firstVisibleItem));

            if (isFirstVisibleItemSection)
            {
                View sectionView = GetChildAt(0);
                if (sectionView.Top == PaddingTop)
                { // view sticks to the top, no need for pinned shadow
                    DestroyPinnedShadow();
                }
                else
                { // section doesn't stick to the top, make sure we have a pinned shadow
                    EnsureShadowForPosition(firstVisibleItem, firstVisibleItem, visibleItemCount);
                }

            }
            else
            { // section is not at the first visible position
                int sectionPosition = FindCurrentSectionPosition(firstVisibleItem);
                if (sectionPosition > -1)
                { // we have section position
                    EnsureShadowForPosition(sectionPosition, firstVisibleItem, visibleItemCount);
                }
                else
                { // there is no section for the first visible item, destroy shadow
                    DestroyPinnedShadow();
                }
            }

        }

        #endregion

        #region OnScroll State Changed

        public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
        {
            if (mDelegateOnScrollListener != null)
            {
                mDelegateOnScrollListener.OnScrollStateChanged(view, scrollState);
            }
        }

        #endregion

        #region On Set Scroll Listner

        public override void SetOnScrollListener(IOnScrollListener l)
        {
            if (l == this)
            {
                base.SetOnScrollListener(l);
            }
            else
            {
                mDelegateOnScrollListener = l;
            }
        }

        #endregion

        #region On Restore Instance State

        public override void OnRestoreInstanceState(Android.OS.IParcelable state)
        {
            base.OnRestoreInstanceState(state);
            Post(new RunnableHolder(RecreatePinnedShadow));
        }

        #endregion

        #region Adapter Property

        public override IListAdapter Adapter
        {
            get
            {
                if (Config.Debug && Adapter != null)
                {
                    if (!(Adapter.GetType().IsAssignableFrom(typeof(IPinnedSectionListAdapter))))
                        throw new IllegalArgumentException("Does your adapter implement PinnedSectionListAdapter?");
                    if (Adapter.ViewTypeCount < 2)
                        throw new IllegalArgumentException("Does your adapter handle at least two types" +
                                " of views in getViewTypeCount() method: items and sections?");
                }
                return base.Adapter;
            }
            set
            {
                base.Adapter = value;
            }
        }

        #endregion

        #region On Layout

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (mPinnedSection != null)
            {
                int parentWidth = right - left - PaddingLeft - PaddingRight;
                int shadowWidth = mPinnedSection.ViewHolder.Width;
                if (parentWidth != shadowWidth)
                {
                    RecreatePinnedShadow();
                }
            }
        }

        #endregion

        #region On Dispatch

        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);
            if (mPinnedSection != null)
            {

                // prepare variables
                int pLeft = ListPaddingLeft;
                int pTop = ListPaddingTop;
                View view = mPinnedSection.ViewHolder;

                // draw child
                canvas.Save();

                int clipHeight = view.Height +
                        (mShadowDrawable == null ? 0 : System.Math.Min(mShadowHeight, mSectionsDistanceY));
                canvas.ClipRect(pLeft, pTop, pLeft + view.Width, pTop + clipHeight);

                canvas.Translate(pLeft, pTop + mTranslateY);
                DrawChild(canvas, mPinnedSection.ViewHolder, DrawingTime);

                if (mShadowDrawable != null && mSectionsDistanceY > 0)
                {
                    mShadowDrawable.SetBounds(mPinnedSection.ViewHolder.Left,
                            mPinnedSection.ViewHolder.Bottom,
                            mPinnedSection.ViewHolder.Right,
                            mPinnedSection.ViewHolder.Bottom + mShadowHeight);
                    mShadowDrawable.Draw(canvas);
                }

                canvas.Restore();
            }
        }

        #endregion

        #region Dispatched Touch Event

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            float x = e.GetX();
            float y = e.GetY();
            MotionEventActions action = e.Action;

            if (action == MotionEventActions.Down
                    && mTouchTarget == null
                    && mPinnedSection != null
                    && IsPinnedViewTouched(mPinnedSection.ViewHolder, x, y))
            { // create touch target

                // user touched pinned view
                mTouchTarget = mPinnedSection.ViewHolder;
                mTouchPoint.X = x;
                mTouchPoint.Y = y;

                // copy down event for eventually be used later
                mDownEvent = MotionEvent.Obtain(e);
            }

            if (mTouchTarget != null)
            {
                if (IsPinnedViewTouched(mTouchTarget, x, y))
                { // forward event to pinned view
                    mTouchTarget.DispatchTouchEvent(e);
                }

                if (action == MotionEventActions.Up)
                { // perform onClick on pinned view
                    base.DispatchTouchEvent(e);
                    PerformPinnedItemClick();
                    ClearTouchTarget();

                }
                else if (action == MotionEventActions.Cancel)
                { // cancel
                    ClearTouchTarget();

                }
                else if (action == MotionEventActions.Move)
                {
                    if (System.Math.Abs(y - mTouchPoint.Y) > mTouchSlop)
                    {

                        // cancel sequence on touch target
                        MotionEvent eventM = MotionEvent.Obtain(e);
                        eventM.Action = MotionEventActions.Cancel;
                        mTouchTarget.DispatchTouchEvent(eventM);
                        eventM.Recycle();

                        // provide correct sequence to super class for further handling
                        base.DispatchTouchEvent(mDownEvent);
                        base.DispatchTouchEvent(e);
                        ClearTouchTarget();

                    }
                }

                return true;
            }
            return base.DispatchTouchEvent(e);
        }

        #endregion

        #region Is Pinned View Touched

        private bool IsPinnedViewTouched(View view, float x, float y)
        {
            view.GetHitRect(mTouchRect);

            // by taping top or bottom padding, the list performs on click on a border item.
            // we don't add top padding here to keep behavior consistent.
            mTouchRect.Top += mTranslateY;

            mTouchRect.Bottom += mTranslateY + PaddingTop;
            mTouchRect.Left += PaddingLeft;
            mTouchRect.Right -= PaddingRight;
            return mTouchRect.Contains((int)x, (int)y);
        }

        #endregion

        #region Clear Touch Target

        private void ClearTouchTarget()
        {
            mTouchTarget = null;
            if (mDownEvent != null)
            {
                mDownEvent.Recycle();
                mDownEvent = null;
            }
        }

        #endregion

        #region Perform Item CLick

        private bool PerformPinnedItemClick()
        {
            if (mPinnedSection == null) return false;

            IOnItemClickListener listener = OnItemClickListener;
            if (listener != null && Adapter.IsEnabled(mPinnedSection.Position))
            {
                View view = mPinnedSection.ViewHolder;
                PlaySoundEffect(SoundEffects.Click);
                if (view != null)
                {
                    view.SendAccessibilityEvent(Android.Views.Accessibility.EventTypes.ViewClicked);
                }
                listener.OnItemClick(this, view, mPinnedSection.Position, mPinnedSection.ID);
                return true;
            }
            return false;
        }

        #endregion
    }

    public class MyGestureDetector : GestureDetector.SimpleOnGestureListener
    { }
    public class RunnableHolder : Java.Lang.Object, IRunnable
    {
        private readonly Action _Run;
        public RunnableHolder(Action run)
        {
            _Run = run;
        }

        public void Run()
        {
            _Run();
        }
    }

    public class PRADataSetObserver : DataSetObserver 
    {
        private readonly Action _OnChanged;
        private readonly Action _OnIvalidated;
        public PRADataSetObserver(Action OnChanged, Action OnInvalidate)
        {
            _OnChanged = OnChanged;
            _OnIvalidated=OnInvalidate;
        }
        public override void OnChanged()
        {
            _OnChanged();
        }
        public override void OnInvalidated()
        {
            _OnIvalidated();
        }
    }

    public interface IPinnedSectionListAdapter 
    {
        bool IsItemViewTypePinned(int viewType);
    }

    public class PinnedSection
    {
        public View ViewHolder { get; set; }
        public int Position { get; set; }

        //TODO : Make generic
        public long ID { get; set; }

    }
}

