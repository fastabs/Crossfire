using UnityEngine;

namespace Crossfire.Workspace
{
    public interface IPlayerMovementService
    {
        void Move(Vector2 delta);
        void RotateY(float value);
        void RotateX(float value);
        void SetDirectRotation(Vector2 value);
    }

    public sealed class PlayerMovementService : IPlayerMovementService
    {
        private static readonly int MoveSpeedParameter = Animator.StringToHash("MoveSpeed");

        private readonly IConfigRepository _configRepository;
        private readonly IPlayerStatsProvider _statsProvider;
        private readonly Player _player;

        private Vector2 _rotation;

        public PlayerMovementService(IConfigRepository configRepository,
            IPlayerStatsProvider statsProvider, Player player)
        {
            _configRepository = configRepository;
            _statsProvider = statsProvider;
            _player = player;

            _rotation = _player.transform.localEulerAngles;
        }

        public void Move(Vector2 direction)
        {
            var moveSpeed = _statsProvider.MoveSpeed.Value;

            var sqrMagnitude = direction.sqrMagnitude;
            _player.Animator.SetFloat(MoveSpeedParameter, sqrMagnitude * moveSpeed);

            if (sqrMagnitude < 0.01f)
                return;

            var scaledMoveSpeed = moveSpeed * Time.fixedDeltaTime;
            var move = Quaternion.Euler(0, _player.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);

            if (_player.Rigidbody != null)
            {
                var newPosition = _player.Rigidbody.position + move * scaledMoveSpeed;
                _player.Rigidbody.MovePosition(newPosition);
            }
            else
            {
                _player.transform.position += move * scaledMoveSpeed;
            }
        }

        public void RotateY(float rotate)
        {
            if (Mathf.Abs(rotate) < 0.01f)
                return;

            var rotationSpeed = _configRepository.Player.RotationSpeed;
            var scaledRotateSpeed = rotationSpeed * Time.fixedDeltaTime;

            _rotation.y += rotate * scaledRotateSpeed;

            var targetRotation = Quaternion.Euler(0, _rotation.y, 0);
            if (_player.Rigidbody != null)
                _player.Rigidbody.MoveRotation(targetRotation);
            else
                _player.transform.rotation = targetRotation;
        }

        public void RotateX(float rotate)
        {
            if (Mathf.Abs(rotate) < 0.01f)
                return;

            var rotationSpeed = _configRepository.Player.RotationSpeed;
            var scaledRotateSpeed = rotationSpeed * Time.deltaTime;

            _rotation.x = Mathf.Clamp(_rotation.x - rotate * scaledRotateSpeed, -89, 89);

            var head = _player.Head != null ? _player.Head : _player.transform;
            var headEuler = head.localEulerAngles;
            headEuler.x = _rotation.x;
            head.localEulerAngles = headEuler;
        }

        public void SetDirectRotation(Vector2 value)
        {
            _rotation = value;

            var targetRotation = Quaternion.Euler(0, _rotation.y, 0);
            if (_player.Rigidbody != null)
                _player.Rigidbody.MoveRotation(targetRotation);
            else
                _player.transform.rotation = targetRotation;

            _rotation.x = Mathf.Clamp(_rotation.x, -89, 89);

            var head = _player.Head != null ? _player.Head : _player.transform;
            var headEuler = head.localEulerAngles;
            headEuler.x = _rotation.x;
            head.localEulerAngles = headEuler;
        }
    }
}

