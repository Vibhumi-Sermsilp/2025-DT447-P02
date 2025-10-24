using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (DialogueManager.Instance.IsOngoing)
			{
				MoveInput(Vector3.zero);
				return;
			}

            if (GameManager.Instance.IsPaused())
            {
                MoveInput(Vector3.zero);
                return;
            }

            MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
            if (DialogueManager.Instance.IsOngoing)
            {
                LookInput(Vector2.zero);
                return;
            }

            if (GameManager.Instance.IsPaused())
            {
                LookInput(Vector2.zero);
                return;
            }

            if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
            if (DialogueManager.Instance.IsOngoing) return;

			if (GameManager.Instance.IsPaused()) return;

            JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
            if (DialogueManager.Instance.IsOngoing) return;

            if (GameManager.Instance.IsPaused()) return;

            SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}