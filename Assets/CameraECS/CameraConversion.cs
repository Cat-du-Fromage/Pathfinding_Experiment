using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CameraECS.Data.Authoring
{
    [DisallowMultipleComponent]
    public class CameraConversion : MonoBehaviour, IConvertGameObjectToEntity
    {
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Right;
        public KeyCode Left;

        public KeyCode LeftShift;
        public int MiddleMouse;

        public float Speed;
        public float ZoomSpeed;
        //public int ShiftMultiplicator;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent<Tag.CameraTag>(entity);
            dstManager.AddComponentData(entity, new Inputs.Up { UpKey = Up });
            dstManager.AddComponentData(entity, new Inputs.Down { DownKey = Down });
            dstManager.AddComponentData(entity, new Inputs.Right { RightKey = Right });
            dstManager.AddComponentData(entity, new Inputs.Left { LeftKey = Left });

            dstManager.AddComponentData(entity, new Inputs.LeftShift { LeftShiftKey = LeftShift });
            dstManager.AddComponentData(entity, new Inputs.MouseMiddle { MiddleMouseKey = MiddleMouse });

            dstManager.AddComponentData(entity, new Move.Direction { Value = new float3(0, 0, 0) });
            dstManager.AddComponentData(entity, new Move.Speed { Value = Speed });
            dstManager.AddComponentData(entity, new Move.SpeedZoom { Value = ZoomSpeed });
            //dstManager.AddComponentData(entity, new Move.ShiftMultiplicator { Value = ShiftMultiplicator });
            dstManager.AddComponent<Move.MouseDragPosition>(entity);
        }
    }
}
