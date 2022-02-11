using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] HumanoidLandInput _input;
    private Animator _animator;

    private void Start()
    {
        _animator = this.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
    }
}
