using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// this script is put on ScrollView object
/// </summary>
public class ScrollControl_horizontal : MonoBehaviour, IEndDragHandler, IBeginDragHandler {

    [Tooltip("element的父物件，此物件必須含'GridLayoutGroup'component (necessary)")]
    public GameObject _content;
    [Tooltip("跳至下一頁按鈕 (optional)")]
    public Button _BtnNext;
    [Tooltip("跳至上一頁按鈕 (optional)")]
    public Button _BtnPrev;
    [Tooltip("加入Scrollbar作為拖曳或顯示 (optional)")]
    public Scrollbar _scrollbar;

    RectTransform _rtContent;
    GridLayoutGroup _lgContent;
    float _contentWigth;
    int _elementInHorizontal = 0;    //一列中有多少element, 借此推出content總長
    int _curPage = 0;                //頁面數(0開始至_elementInHorizontal - _elementInView)

    //lerp
    [Tooltip("滑動速度 (must>0)")]
    public float _elasticityStrength;    //滑動強度
    bool _lerp;
    Vector2 _LerpTarget;
    PointerEventData pointData;

    //viewSetting
    [Tooltip("ViewPoint中可見的element欄數 (must>0)")]
    public int _elementInView;           //一次可以看到多少欄, 判斷page超過與否

    //Pagination
    public GameObject _pagination;

    void Start () {
        _rtContent = _content.GetComponent<RectTransform>();
        _lgContent = _content.GetComponent<GridLayoutGroup>();

        if (_BtnNext)
            _BtnNext.GetComponent<Button>().onClick.AddListener(() => { NextPage(); });
        if (_BtnPrev)
            _BtnPrev.GetComponent<Button>().onClick.AddListener(() => { PreviousPage(); });
        if (_scrollbar) {
            EventListener.Get(_scrollbar.gameObject).onUp = OnUp;
            EventListener.Get(_scrollbar.gameObject).onDown = OnDown;
        }

        AdjustContentWidth();
        SetPagination(0);
    }
	
	void Update () {
        //滑至定位
        if (_lerp) {
            _rtContent.offsetMin = Vector2.Lerp(_rtContent.offsetMin, _LerpTarget, _elasticityStrength * Time.deltaTime);
            _rtContent.sizeDelta = new Vector2(_contentWigth, 0);
            if (Vector2.Distance(_rtContent.offsetMin, _LerpTarget) < 0.05f) {
                _lerp = false;
                Debug.Log("end lerp");
            }
        }
       
    }
    /// <summary>
    /// 調整content長度: 求得一列中有多少element, 借此推出content總長
    /// </summary>
    void AdjustContentWidth() {
        int totalElement = _content.transform.childCount;   //總子物件數
        int constraintCount = _lgContent.constraintCount;   //鎖定數(不管欄或列)

        //鎖定列數(row)
        if (_lgContent.constraint == GridLayoutGroup.Constraint.FixedRowCount) {
            int quo = (int)(totalElement / constraintCount);    //quotient, 商數
            int rem = totalElement % constraintCount;           //remainder, 餘數
            if (quo > 0) {
                if (rem != 0) { _elementInHorizontal = quo + 1; } else _elementInHorizontal = quo;
            } else _elementInHorizontal = 1;
        }
        //鎖定欄數(Column)
        if (_lgContent.constraint == GridLayoutGroup.Constraint.FixedColumnCount) {
            if (totalElement >= constraintCount) {
                _elementInHorizontal = constraintCount;
            } else _elementInHorizontal = totalElement;
        }

        //set _contentWigth
        _contentWigth = _elementInHorizontal * (_lgContent.cellSize.x + _lgContent.spacing.x);
        _rtContent.sizeDelta = new Vector2(_contentWigth, _rtContent.sizeDelta.y);
        Debug.Log("_contentWigth: " + _contentWigth);
        Debug.Log("elementInHorizontal: " + _elementInHorizontal);

    }
    public void Btn_Click() {
        Debug.Log("Click");
    }
    public void OnBeginDrag(PointerEventData eventData) {
        //Debug.Log("Start drag");
        _lerp = false;
    }
    public void OnEndDrag(PointerEventData eventData) {
        int targetPage;
        float pagePoint;    //頁面判定分界點
        float contentPosi = _rtContent.offsetMin.x;     //目前content位置

        pagePoint = _lgContent.cellSize.x / 2 + _lgContent.spacing.x / 2;
        targetPage = (int)(((contentPosi + pagePoint) / (_lgContent.cellSize.x + _lgContent.spacing.x)) * -1 + 1);

        SetPage(targetPage);
    }
    //for scrollBar
    void OnUp(GameObject button) {
        int targetPage;
        float pagePoint;    //頁面判定分界點
        float contentPosi = _rtContent.offsetMin.x;     //目前content位置

        pagePoint = _lgContent.cellSize.x / 2 + _lgContent.spacing.x / 2;
        targetPage = (int)(((contentPosi + pagePoint) / (_lgContent.cellSize.x + _lgContent.spacing.x)) * -1 + 1);

        SetPage(targetPage);
    }
    void OnDown(GameObject button) {
        _lerp = false;
    }
    void SetPage(int targetPageNo) {
        _curPage = targetPageNo;
        //large of range
        if (_curPage > _elementInHorizontal - _elementInView) {
            _curPage = _elementInHorizontal - _elementInView;
        }
        //small of range
        if (_curPage < 0) {
            _curPage = 0;
        }
        Debug.Log("page:" + _curPage);
        //set lerp target
        _LerpTarget = new Vector2((_curPage * (_lgContent.cellSize.x + _lgContent.spacing.x)) * -1, 0);

        //start lerp
        _lerp = true;
        //start SetPagination
        SetPagination(_curPage);
    }
    void NextPage() {
        SetPage(_curPage + 1);
    }
    void PreviousPage() {
        SetPage(_curPage - 1);
    }
    void SetPagination(int curPage) {
        if (_pagination) {
            for (int i = 0; i < _pagination.transform.childCount; i++) {
                _pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (curPage == i)
                    ? true
                    : false;
            }
        }
    }

}
