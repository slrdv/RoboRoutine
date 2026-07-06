using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class GridController : ISnapshotable, IDisposable
    {
        private readonly GridModel _gridModel;
        private readonly GridView _gridView;
        private readonly ISnapshotableRegistry _snapshotableRegistry;
        private readonly Dictionary<Vector2Int, IGridEntityController> _entities = new();

        private readonly List<Vector2Int> _cachedKeys = new();

        public SnapshotLayer SnapshotLayer => SnapshotLayer.Grid;

        public GridController(GridModel gridModel, GridView gridView, ISnapshotableRegistry snapshotableRegistry)
        {
            _gridModel = gridModel;
            _gridView = gridView;
            _snapshotableRegistry = snapshotableRegistry;

            _snapshotableRegistry.Register(this);
        }

        public Vector2 GetCellCenterWorldPositionXZ(Vector2Int cell)
        {
            return new Vector2(cell.x + 0.5f, cell.y + 0.5f);
        }

        public Vector3 GetCellCenterWorldPosition(Vector2Int cell)
        {
            return GetCellCenterWorldPositionXZ(cell).WithY(_gridView.GetPosition().y);
        }

        public Vector3 GetFootprintCenterWorldPosition(Vector2Int origin, Vector2Int size)
        {
            return new Vector3(origin.x + size.x * 0.5f, _gridView.GetPosition().y, origin.y + size.y * 0.5f);
        }

        public bool TryFindAtWorldPositionXZ(Vector2 worldPositionXZ, out IGridEntityController item)
        {
            Vector2Int cell = WorldToCell(worldPositionXZ);

            if (!_gridModel.TryGetEntity(cell, out GridEntityModel model))
            {
                item = null;
                return false;
            }

            return TryGetEntity(model, out item);
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            return WorldToCell(worldPosition.ToXZ());
        }

        public Vector2Int WorldToCell(Vector2 worldPositionXZ)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPositionXZ.x), Mathf.FloorToInt(worldPositionXZ.y));
        }

        public void AddEntity(IGridEntityController entity, Vector2Int origin)
        {
            _gridModel.AddEntity(entity.Model, origin);
            _entities[origin] = entity;
            entity.AttachToParent(_gridView.transform, GetFootprintCenterWorldPosition(origin, entity.Model.Size), Vector3.one);
        }

        public bool TryAddEntity(IGridEntityController entity, Vector2Int position)
        {
            if (_gridModel.IsOccupied(position)) return false;

            AddEntity(entity, position);

            return true;
        }

        public void RemoveEntity(IGridEntityController entity)
        {
            if (!TryGetCell(entity, out Vector2Int cell)) throw new ArgumentException($"Entity not found");
            RemoveEntity(cell);
        }

        public void RemoveEntity(Vector2Int position)
        {
            _entities.Remove(position);
            _gridModel.RemoveEntity(position);
        }

        public void Build()
        {
            _gridView.Build(_gridModel.Bounds);
        }

        public object CaptureState()
        {
            return new GridHistoryState { Entities = new Dictionary<Vector2Int, IGridEntityController>(_entities) };
        }

        public void RestoreState(object state)
        {
            if (state is not GridHistoryState gridState) throw new ArgumentException($"Invalid history state type: {state.GetType().Name}");

            _cachedKeys.Clear();
            foreach (var kv in _entities)
            {
                _cachedKeys.Add(kv.Key);
            }

            for (int i = 0; i < _cachedKeys.Count; i++)
            {
                Vector2Int key = _cachedKeys[i];
                if (!gridState.Entities.TryGetValue(key, out var target) || target != _entities[key])
                {
                    Debug.Log($"[{GetType().Name}.{nameof(RestoreState)}] Remove {_entities[key].View.name} at {key}");
                    RemoveEntity(key);
                }
            }

            foreach (var kv in gridState.Entities)
            {
                if (!_entities.TryGetValue(kv.Key, out var current) || current != kv.Value)
                {
                    Debug.Log($"[{GetType().Name}.{nameof(RestoreState)}] Restore {gridState.Entities[kv.Key].View.name} at {kv.Key}");
                    AddEntity(kv.Value, kv.Key);
                }
            }
        }

        public void Dispose()
        {
            _snapshotableRegistry.Remove(this);
        }

        private bool TryGetCell(IGridEntityController entity, out Vector2Int origin)
        {
            foreach (var kv in _entities)
            {
                if (kv.Value == entity)
                {
                    origin = kv.Key;
                    return true;
                }
            }

            origin = default;
            return false;
        }

        private bool TryGetEntity(GridEntityModel model, out IGridEntityController entity)
        {
            foreach (var kv in _entities)
            {
                if (kv.Value.Model == model)
                {
                    entity = kv.Value;
                    return true;
                }
            }

            entity = null;
            return false;
        }
    }
}