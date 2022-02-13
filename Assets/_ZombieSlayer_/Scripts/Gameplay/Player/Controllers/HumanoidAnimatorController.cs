using UnityEngine;

public class HumanoidAnimatorController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] HumanoidLandInput _input;

    private void FixedUpdate()
    {
        float _horizontalContainer = _input.MoveInput.normalized.x;
        float _verticalContainer = _input.MoveInput.normalized.y;
        if (!_input.moveIsPressed && !_input.JumpIsPressed)
        {
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isMoving", false);
        }
        else if(!_input.moveIsPressed && _input.JumpIsPressed)
        {
            _animator.SetBool("isJumping", true);
        }
        else if (_input.moveIsPressed && !_input.JumpIsPressed)
        {
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isMoving", true);
            if(_input.RunIsPressed)
            {
                _verticalContainer *= 2;
                _horizontalContainer *= 2;
            }
        }
        else if (_input.moveIsPressed && _input.JumpIsPressed)
        {
            _animator.SetBool("isMoving", true);
            _animator.SetBool("isJumping", true);
            _animator.SetFloat("Horizontal", _input.MoveInput.x);
            _animator.SetFloat("Vertical", _input.MoveInput.y);
        }
        else
        {
            _animator.SetBool("isMoving", false);
            _animator.SetBool("isJumping", false);
        }
        _animator.SetFloat("Horizontal", _horizontalContainer);
        _animator.SetFloat("Vertical", _verticalContainer);

    }
}
