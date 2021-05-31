using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Diagnostics;

using CamMove = CameraECS.Data.Move;
using CamInput = CameraECS.Data.Inputs;
namespace CameraECS.InputSystem
{

    [BurstCompile]
    public class CameraSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<Data.Tag.CameraTag>();
        }
        protected override void OnUpdate()
        {
            EntityQuery test = GetEntityQuery(typeof(CamMove.MouseStartPosition));
            Entities
                //.WithBurst()
                .WithoutBurst()
                .WithAll<Data.Tag.CameraTag>()
                .ForEach((Entity camera, ref CamMove.Direction direction, in CamInput.LeftShift leftShift, in CamInput.Up up, in CamInput.Down down, in CamInput.Right right, in CamInput.Left left, in CamInput.MouseMiddle midMouse) =>
                {
                    float3 x = (Input.GetKey(right.RightKey) ? math.right() : float3.zero) + (Input.GetKey(left.LeftKey) ? math.left() : float3.zero);
                    float3 z = (Input.GetKey(up.UpKey) ? math.forward() : float3.zero) + (Input.GetKey(down.DownKey) ? math.back() : float3.zero);

                    // Y axe is a bit special
                    float3 y = float3.zero;
                    if (!Input.mouseScrollDelta.Equals(float2.zero)) { y = Input.mouseScrollDelta.y > 0 ? math.up() : math.down(); }

                    direction.Value = x + y + z;

                    //Rotation
                    if (Input.GetKeyDown(midMouse.MiddleMouseKey)) SetComponent(camera, new CamMove.MouseStartPosition { Value = Input.mousePosition });
                    if(Input.GetKey(midMouse.MiddleMouseKey)) test.SetChangedVersionFilter(typeof(CamMove.MouseStartPosition));
                    //if (Input.GetKey(midMouse.MiddleMouseKey)) Change
                }).Run();
        }
    }

    [BurstCompile]
    public class CameraMoveSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<Data.Tag.CameraTag>();
        }
        protected override void OnUpdate()
        {
            bool didMove = !GetComponent<CamMove.Direction>(GetSingletonEntity<Data.Tag.CameraTag>()).Value.Equals(float3.zero);
            float deltaTime = Time.DeltaTime;

            if (didMove)
            {
                Entities
                    .WithBurst()
                    .ForEach((ref Translation position, ref Rotation rotation, ref CamMove.Speed speed, ref CamMove.SpeedZoom speedZoom, in CamMove.Direction direction, in CamInput.LeftShift leftShift, in CamMove.ShiftMultiplicator shiftMul) =>
                    {
                        #region X/Z translation
                        //Shift Key multiplicator
                        float speedXZ = Input.GetKey(leftShift.LeftShiftKey) ? math.mul(speed.Value, shiftMul.Value) : speed.Value; //speed
                        float speedY = Input.GetKey(leftShift.LeftShiftKey) ? math.mul(speedZoom.Value, shiftMul.Value) : speedZoom.Value; //speedZoom

                        //Speed depending on Y Position (min : default speed Value)
                        speedXZ = math.max(speedXZ, math.mul(position.Value.y, speedXZ));
                        speedY = math.max(speedY, math.mul( math.log(position.Value.y), speedY ) );

                        //Dependency with delta time
                        float SpeedDeltaTimeXZ = math.mul(speedXZ, deltaTime);
                        float SpeedDeltaTimeY = math.mul(speedY, deltaTime);

                        //calculate new position (both XZ and Y)
                        float3 HorizontalMove = new float3(math.mad(direction.Value.x, SpeedDeltaTimeXZ, position.Value.x), 0, math.mad(direction.Value.z, SpeedDeltaTimeXZ, position.Value.z));
                        float3 VerticalMove = new float3(0, math.mad(direction.Value.y, SpeedDeltaTimeY, position.Value.y), 0);

                        position.Value = HorizontalMove + VerticalMove;
                        #endregion X/Z translation

                        #region Rotation

                        //rotation.Value = quaternion.EulerZXY(new float3(1,1,1));
                        #endregion Rotation

                    }).Run();
            }
        }
    }

    
    [BurstCompile]
    public class CameraRotationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<Data.Tag.CameraTag>();
        }
        protected override void OnUpdate()
        {
            float deltaTime = Time.DeltaTime;

            bool firstSpawn = !GetComponent<CamMove.MouseStartPosition>(GetSingletonEntity<Data.Tag.CameraTag>()).Value.Equals(float3.zero);
            if (firstSpawn)
            {
                Entities
                    .WithBurst()
                    .WithChangeFilter<CamMove.MouseStartPosition>()
                    .ForEach((ref Rotation rotation, in CamMove.MouseStartPosition startPosition) =>
                    {
                        UnityEngine.Debug.Log($"New position : {startPosition.Value}");
                        #region Rotation

                        //rotation.Value = quaternion.EulerZXY(new float3(1,1,1));
                        #endregion Rotation
                    }).Run();
            }
        }
    }
    
}
