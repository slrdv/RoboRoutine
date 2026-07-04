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

        public bool TryGetAtWorldPositionXZ(Vector2 worldPositionXZ, out IGridEntityController item)
        {
            Vector2Int cell = WorldToCell(worldPositionXZ);
            if (!_gridModel.isOccupied(cell))
            {
                item = null;
                return false;
            }

            item = _entities[cell];
            return true;
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            return WorldToCell(worldPosition.ToXZ());
        }

        public Vector2Int WorldToCell(Vector2 worldPositionXZ)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPositionXZ.x), Mathf.FloorToInt(worldPositionXZ.y));
        }

        public void AddEntity(IGridEntityController entity, Vector2Int position)
        {
            _gridModel.AddEntity(entity.Model, position);
            _entities[position] = entity;
            _gridView.Attach(entity.View, GetCellCenterWorldPosition(position));
        }

        public bool TryAddEntity(IGridEntityController entity, Vector2Int position)
        {
            if (_gridModel.isOccupied(position)) return false;
            
            AddEntity(entity, position);

            return true;
        }

        public void RemoveEntity(IGridEntityController entity)
        {
            RemoveEntity(_gridModel.GetPosition(entity.Model));
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
            return new GridState { Entities = new Dictionary<Vector2Int, IGridEntityController>(_entities) };
        }

        public void RestoreState(object state)
        {
            if (state is not GridState gridState) throw new ArgumentException($"Invalid state type: {state.GetType().Name}");

            HashSet<Vector2Int> oldKeys = new(_entities.Keys);
            HashSet<Vector2Int> newKeys = new(gridState.Entities.Keys);

            foreach (Vector2Int key in newKeys)
            {
                if (!oldKeys.Contains(key))
                {
                    AddEntity(gridState.Entities[key], key);
                }
            }

            foreach (Vector2Int key in oldKeys)
            {
                if (!newKeys.Contains(key))
                {
                    RemoveEntity(key);
                }
            }
        }

        public void Dispose()
        {
            _snapshotableRegistry.Remove(this);
        }
    }
}