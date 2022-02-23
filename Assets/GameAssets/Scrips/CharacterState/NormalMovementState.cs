using Base;
using Base.MessageSystem;
using Base.Module;
using Base.Pattern;
using UnityEngine;

namespace Game
{
    public class NormalMovementState : CharacterState
    {
        [SerializeField] private float speedLerpRotate = .1f;
        [SerializeField] private float moveSpeed = 1;
        
        private Vector3 _posClickOld;
        private Vector3 _deltaClick;
        private Vector3 _lookDir;
        private int _horizontalMove;

        private PlayerController _playerController;

        protected override void Start()
        {
            base.Start();

            _playerController = this.GetComponentInBranch<PlayerController>();
            
            Messenger.RegisterListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        public override void UpdateBehaviour(float dt)
        {
            if (_horizontalMove > 0)
            {
                HandlingMovement();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            Messenger.RemoveListener<InputPhase, Vector3>(SystemMessage.Input, OnInputResponse);
        }

        private void OnInputResponse(InputPhase phase, Vector3 inputPos)
        {
            switch (phase)
            {
                case InputPhase.Began:
                    _posClickOld = inputPos;
                    _horizontalMove = 1;
                    break;
                case InputPhase.Moved:
                    _deltaClick = inputPos - _posClickOld;
                    _posClickOld = inputPos;
                    _horizontalMove = 1;
                    break;
                case InputPhase.Ended:
                    _deltaClick = Vector3.zero;
                    _lookDir = Vector3.zero;
                    _horizontalMove = 0;
                    break;
                default:
                    break;
            }
        
            HandlingInput();
        }
        
        private void HandlingInput()
        {
            if (_deltaClick != Vector3.zero)
            {
                Vector3 deltaTouch = _deltaClick;
                deltaTouch.z = deltaTouch.y;
                deltaTouch.y = 0;

                _lookDir += deltaTouch;
                _lookDir = Vector3.ClampMagnitude(_lookDir, 150f);
            }

            if (_lookDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(_lookDir.x, _lookDir.z) * Mathf.Rad2Deg;
                //float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _turnSmoothTime, speedLerpRotate);
                //transform.rotation = Quaternion.Euler(0f, smoothAngle, 0);
                _playerController.Rotation = Quaternion.Lerp(_playerController.Rotation, Quaternion.Euler(0f, angle, 0), speedLerpRotate);
            }
        }
        
        private void HandlingMovement()
        {
            float angle = Mathf.Atan2(_lookDir.x, _lookDir.z) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 forward = CacheTransform.forward * _horizontalMove;
            // if (_characterController.isGrounded == false)
            // {
            //     forward.y += Physics.gravity.y;
            // }
            // else
            // {
            //     forward.y = 0;
            // }
            _playerController.CharacterMove(forward * (Time.fixedDeltaTime * moveSpeed));
        }
    }
}

