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
    private RectTransform maskRect; // ����

    void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        maskRect = minimap.transform.parent.GetComponent<RectTransform>(); // ��ȡ���ֳߴ�
    }

    void Update() => HandleZoomInput();

    void HandleZoomInput()
    {
        if (!isMouseOver) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        // ��ȡ���ֲ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimap.rectTransform,
            Input.mousePosition,
            parentCanvas.worldCamera,
            out Vector2 localPoint);

        // ���������ű���
        float newScale = Mathf.Clamp(
            minimap.rectTransform.localScale.x * (1 + (scroll > 0 ? zoomSpeed : -zoomSpeed)),
            minZoom,
            maxZoom
        );

        // ����λ��ƫ�Ʋ�Ӧ������
        Vector2 targetPos = CalculateClampedPosition(localPoint, newScale);
        minimap.rectTransform.localScale = Vector3.one * newScale;
        minimap.rectTransform.anchoredPosition = targetPos;
    }

    Vector2 CalculateClampedPosition(Vector2 mouseLocalPos, float currentScale)
    {
        // ��ȡ���ֺ�С��ͼ��ʵ�ʳߴ�
        float maskWidth = maskRect.rect.width;
        float maskHeight = maskRect.rect.height;
        float mapWidth = minimap.rectTransform.rect.width * currentScale;
        float mapHeight = minimap.rectTransform.rect.height * currentScale;

        // �����������ƫ����
        float maxX = (mapWidth - maskWidth) / 2;
        float maxY = (mapHeight - maskHeight) / 2;

        // ����Ԥ��λ��
        Vector2 expectedPos = minimap.rectTransform.anchoredPosition - (mouseLocalPos * (currentScale / minimap.rectTransform.localScale.x - 1));

        // Ӧ�ñ߽�����
        return new Vector2(
            Mathf.Clamp(expectedPos.x, -maxX, maxX),
            Mathf.Clamp(expectedPos.y, -maxY, maxY)
        );
    }

    public void OnPointerEnter(PointerEventData eventData) => isMouseOver = true;
    public void OnPointerExit(PointerEventData eventData) => isMouseOver = false;
}