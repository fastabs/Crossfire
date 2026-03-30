using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PlayerInputController : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private readonly InputActions _inputActions;
        private readonly ICursorService _cursorService;
        private readonly IPlayerMovementService _playerMovementService;
        private readonly IPlayerShootingService _playerShootingService;
        private readonly PauseService _pauseService;

        public PlayerInputController(InputActions inputActions, ICursorService cursorService,
            IPlayerMovementService playerMovementService, IPlayerShootingService playerShootingService,
            PauseService pauseService)
        {
            _inputActions = inputActions;
            _playerMovementService = playerMovementService;
            _playerShootingService = playerShootingService;
            _pauseService = pauseService;
            _cursorService = cursorService;
        }

        public void Initialize()
        {
            _inputActions.Enable();
            _inputActions.Player.Fire.performed += OnFire;
            _inputActions.Player.CursorMode.performed += OnCursorModeToggle;
            _cursorService.Hide();
        }

        public void Dispose()
        {
            _inputActions.Player.Fire.performed -= OnFire;
            _inputActions.Player.CursorMode.performed -= OnCursorModeToggle;
            _inputActions.Disable();
        }

        public void Tick()
        {
            if (_pauseService.IsPaused)
                return;

            if (_cursorService.CursorMode != CursorMode.Hidden)
                return;

            var rotate = _inputActions.Player.Rotation.ReadValue<Vector2>();
            _playerMovementService.RotateX(rotate.y);
        }

        public void FixedTick()
        {
            if (_pauseService.IsPaused)
                return;

            var move = _inputActions.Player.Move.ReadValue<Vector2>();
            _playerMovementService.Move(move);

            if (_cursorService.CursorMode != CursorMode.Hidden)
                return;

            var rotate = _inputActions.Player.Rotation.ReadValue<Vector2>();
            _playerMovementService.RotateY(rotate.x);
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            if (_cursorService.CursorMode != CursorMode.Hidden)
                return;

            _playerShootingService.Shoot();
        }

        private void OnCursorModeToggle(InputAction.CallbackContext context)
        {
            _cursorService.Toggle();
            _pauseService.Toggle();
        }
    }
}

