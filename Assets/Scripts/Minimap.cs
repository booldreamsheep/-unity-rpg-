using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minimap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.2f;
    public float minZoom = 0.5f;
    public float maxZoom = 3f;
    public RawImage minimap;

    private bool isMouseOver;
    private Canvas parentCanvas;
    private RectTransform maskRect; // 新增

    void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        maskRect = minimap.transform.parent.GetComponent<RectTransform>(); // 获取遮罩尺寸
    }

    void Update() => HandleZoomInput();

    void HandleZoomInput()
    {
        if (!isMouseOver) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        // 获取鼠标局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimap.rectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera,
            out Vector2 localPoint);

        // 计算新缩放比例
        float newScale = Mathf.Clamp(
            minimap.rectTransform.localScale.x * (1 + (scroll > 0 ? zoomSpeed : -zoomSpeed)),
            minZoom,
            maxZoom
        );

        // 计算位置偏移并应用限制
        Vector2 targetPos = CalculateClampedPosition(localPoint, newScale);
        minimap.rectTransform.localScale = Vector3.one * newScale;
        minimap.rectTransform.anchoredPosition = targetPos;
    }

    Vector2 CalculateClampedPosition(Vector2 mouseLocalPos, float currentScale)
    {
        // 获取遮罩和小地图的实际尺寸
        float maskWidth = maskRect.rect.width;
        float maskHeight = maskRect.rect.height;
        float mapWidth = minimap.rectTransform.rect.width * currentScale;
        float mapHeight = minimap.rectTransform.rect.height * currentScale;

        // 计算最大允许偏移量
        float maxX = (mapWidth - maskWidth) / 2;
        float maxY = (mapHeight - maskHeight) / 2;

        // 计算预期位置
        Vector2 expectedPos = minimap.rectTransform.anchoredPosition - (mouseLocalPos * (currentScale / minimap.rectTransform.localScale.x - 1));

        // 应用边界限制
        return new Vector2(
            Mathf.Clamp(expectedPos.x, -maxX, maxX),
            Mathf.Clamp(expectedPos.y, -maxY, maxY)
        );
    }

    public void OnPointerEnter(PointerEventData eventData) => isMouseOver = true;
    public void OnPointerExit(PointerEventData eventData) => isMouseOver = false;
}