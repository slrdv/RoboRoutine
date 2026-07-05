using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridModel
    {
        private RectInt _bounds = RectInt.zero;
        private readonly Dictionary<Vector2Int, GridEntityModel> _cells = new();

        public RectInt Bounds => _bounds;

        public void AddEntity(GridEntityModel entity, Vector2Int origin)
        {
            for (int x = origin.x; x < origin.x + entity.Size.x; x++)
            {
                for (int y = origin.y; y < origin.y + entity.Size.y; y++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    if (IsOccupied(cell)) throw new ArgumentException($"Cell {cell} is occupied");

                    _cells.Add(cell, entity);
                }
            }

            _bounds.xMin = Mathf.Min(_bounds.xMin, origin.x);
            _bounds.yMin = Mathf.Min(_bounds.yMin, origin.y);
            _bounds.xMax = Mathf.Max(_bounds.xMax, origin.x + entity.Size.x);
            _bounds.yMax = Mathf.Max(_bounds.yMax, origin.y + entity.Size.y);
        }

        public void RemoveEntity(Vector2Int position)
        {
            RemoveEntity(_cells[position]);
        }

        public void RemoveEntity(GridEntityModel entity)
        {
            List<Vector2Int> cells = GetCells(entity);
            if (cells.Count == 0) throw new ArgumentException($"Entity not found");

            for (int i = 0; i < cells.Count; i++)
            {
                _cells.Remove(cells[i]);
            }
        }

        public GridEntityModel GetEntity(Vector2Int cell)
        {
            return _cells[cell];
        }

        public bool TryGetEntity(Vector2Int cell, out GridEntityModel entity)
        {
            return _cells.TryGetValue(cell, out entity);
        }

        public bool IsOccupied(Vector2Int cell)
        {
            return _cells.ContainsKey(cell);
        }

        private List<Vector2Int> GetCells(GridEntityModel entity)
        {
            List<Vector2Int> cells = new(entity.Size.x * entity.Size.y);

            foreach (var kv in _cells)
            {
                if (kv.Value == entity)
                {
                    cells.Add(kv.Key);
                }
            }

            return cells;
        }
    }
}