using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RoboRoutine
{
    public sealed class Arrow : MonoBehaviour
    {
        public event Action<Arrow, PointerEventData> BeginDragEvent;
        public event Action<Arrow, PointerEventData> EndDragEvent;
        public event Action<Arrow, PointerEventData> DragEvent;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private DragPoint _dragPoint;
        [SerializeField] private Image _startPointImage;
        [SerializeField] private Image _line;
        [SerializeField] private float _thickness = 0.5f;

        private Vector2 _startPosition;

        public DragPoint DragPoint => _dragPoint;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public Vector2 DragPointPosition => _dragPoint.RectTransform.position;
        public Vector2 StartPosition => _startPosition;

        public void SetColor(Color color)
        {
            _dragPoint.Image.color = color;
            _line.color = color;
            _startPointImage.color = color;
        }

        public void DrawArrow(Vector2 start, Vector2 end)
        {
            _startPosition = start;
            _dragPoint.RectTransform.position = end;
            _startPointImage.rectTransform.position = start;
            DrawLine(start, end);

            _startPointImage.gameObject.SetActive(true);
            _line.gameObject.SetActive(true);
        }

        public void DrawDragPoint(Vector2 position)
        {
            _startPosition = position;
            _dragPoint.RectTransform.position = position;

            _startPointImage.gameObject.SetActive(false);
            _line.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(this, eventData);
        }

        private void DrawLine(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            RectTransform lineRect = _line.rectTransform;
            lineRect.position = start + direction * 0.5f;
            lineRect.sizeDelta = new Vector2(distance, _thickness);
            lineRect.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Awake()
        {
            _dragPoint.BeginDragEvent += OnBeginDrag;
            _dragPoint.DragEvent += OnDrag;
            _dragPoint.EndDragEvent += OnEndDrag;

            _dragPoint.RectTransform.SetAsLastSibling();
        }

        private void OnDestroy()
        {
            _dragPoint.BeginDragEvent -= OnBeginDrag;
            _dragPoint.DragEvent -= OnDrag;
            _dragPoint.EndDragEvent -= OnEndDrag;
        }
    }
}
