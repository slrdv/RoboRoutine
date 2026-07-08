using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace RoboRoutine
{
    public sealed class ArrowPanelView : MonoBehaviour
    {
        public event Action<Arrow, Vector2> ArrowDropEvent;

        [SerializeField] private RectTransform _arrowsRoot;
        [SerializeField] private Arrow _arrowPrefab;

        private ICanvasService _canvasService;

        private readonly List<Arrow> _arrows = new();
        private bool _isDragging;
        private bool _inputEnabled = true;
        private Vector2 _dragPosition;


        [Inject]
        public void Construct(ICanvasService canvasService)
        {
            _canvasService = canvasService;
        }

        public Arrow CreateArrow()
        {
            Arrow arrow = Instantiate(_arrowPrefab, _arrowsRoot);

            arrow.SetColor(GetRandomColor());
            _arrows.Add(arrow);

            arrow.BeginDragEvent += OnBeginDrag;
            arrow.EndDragEvent += OnEndDrag;
            arrow.DragEvent += OnDrag;

            return arrow;
        }

        public void RemoveArrow(Arrow arrow)
        {
            _arrows.Remove(arrow);
            DestroyArrow(arrow);
        }

        public void DrawArrow(Arrow arrow, Vector2 start, Vector2 end)
        {
            arrow.DrawArrow(GetPositionInItemRoot(start), GetPositionInItemRoot(end));
        }

        public void DrawDragPoint(Arrow arrow, Vector2 position)
        {
            arrow.DrawDragPoint(GetPositionInItemRoot(position));
        }

        public void DestroyAllArrows()
        {
            for (int i = 0; i < _arrows.Count; i++)
            {
                DestroyArrow(_arrows[i]);
            }

            _arrows.Clear();
        }

        public void OnBeginDrag(Arrow arrow, PointerEventData eventData)
        {
            if (!_inputEnabled || _isDragging) return;

            arrow.CanvasGroup.alpha = 0.35f;
            arrow.CanvasGroup.blocksRaycasts = false;

            _dragPosition = arrow.StartPosition;
            _isDragging = true;
        }

        public void OnDrag(Arrow arrow, PointerEventData eventData)
        {
            if (!_inputEnabled || !_isDragging) return;

            arrow.DrawArrow(_dragPosition, arrow.DragPointPosition + eventData.delta);
        }

        public void OnEndDrag(Arrow arrow, PointerEventData eventData)
        {
            if (!_isDragging || !_inputEnabled) return;

            arrow.CanvasGroup.alpha = 1f;
            arrow.CanvasGroup.blocksRaycasts = true;

            _isDragging = false;

            ArrowDropEvent?.Invoke(arrow, eventData.position);
        }

        private void DestroyArrow(Arrow arrow)
        {
            if (arrow == null) return;

            arrow.BeginDragEvent -= OnBeginDrag;
            arrow.EndDragEvent -= OnEndDrag;
            arrow.DragEvent -= OnDrag;

            Destroy(arrow.gameObject);
        }

        private void OnDestroy()
        {
            DestroyAllArrows();
        }

        private Vector2 GetPositionInItemRoot(Vector2 position)
        {
            return new Vector2(_arrowsRoot.position.x, position.y);
        }

        private Color GetRandomColor()
        {
            return UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.7f, 1f, 1f, 1f);
        }
    }
}
