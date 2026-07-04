using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridModel
    {
        private RectInt _bounds = RectInt.zero;
        private readonly Dictionary<Vector2Int, GridEntityModel> _entities = new();

        public RectInt Bounds => _bounds;

        public void AddEntity(GridEntityModel entity, Vector2Int position)
        {
            _entities.Add(position, entity);
            
            _bounds.xMin = Mathf.Min(_bounds.xMin, position.x);
            _bounds.yMin = Mathf.Min(_bounds.yMin, position.y);
            _bounds.xMax = Mathf.Max(_bounds.xMax, position.x + entity.Size.x);
            _bounds.yMax = Mathf.Max(_bounds.yMax, position.y + entity.Size.y);
        }

        public void RemoveEntity(Vector2Int position)
        {
            _entities.Remove(position);
        }

        public void RemoveEntity(GridEntityModel entity)
        {
            RemoveEntity(GetPosition(entity));
        }


        public Vector2Int GetPosition(GridEntityModel entity)
        {
            foreach (var kv in _entities)
            {
                if (kv.Value == entity)
                {
                    return kv.Key;
                }
            }

            throw new ArgumentException("Entity not found");
        }

        public GridEntityModel GetEntity(Vector2Int position)
        {
            return _entities[position];
        }

        public bool isOccupied(Vector2Int position)
        {
            return _entities.ContainsKey(position);
        }
    }
}