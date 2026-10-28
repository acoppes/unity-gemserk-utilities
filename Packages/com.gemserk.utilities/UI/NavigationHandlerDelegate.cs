using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gemserk.Utilities.UI
{
    public class NavigationHandlerDelegate : MonoBehaviour, IMoveHandler
    {
        public enum DelegateType
        {
            Parent = 0,
            Object = 1
        }

        public DelegateType delegateType;
        
        // [ConditionalField(nameof(delegateType), false, DelegateType.Object)]
        public GameObject delegateObject;
        
        public void OnMove(AxisEventData eventData)
        {
            var selectable = GetComponent<Selectable>();

            if (selectable)
            {
                var navigation = selectable.navigation;
                
                if (navigation.mode.HasFlag(Navigation.Mode.Horizontal))
                {
                    if (eventData.moveDir is MoveDirection.Left or MoveDirection.Right)
                    {
                        return;
                    }
                }
                
                if (navigation.mode.HasFlag(Navigation.Mode.Vertical))
                {
                    if (eventData.moveDir is MoveDirection.Down or MoveDirection.Up)
                    {
                        return;
                    }
                }
                
                if (navigation.mode.HasFlag(Navigation.Mode.Explicit))
                {
                    if (eventData.moveDir == MoveDirection.Up && navigation.selectOnUp)
                    {
                        return;
                    }
                    
                    if (eventData.moveDir == MoveDirection.Down && navigation.selectOnDown)
                    {
                        return;
                    }
                    
                    if (eventData.moveDir == MoveDirection.Left && navigation.selectOnLeft)
                    {
                        return;
                    }
                    
                    if (eventData.moveDir == MoveDirection.Right && navigation.selectOnRight)
                    {
                        return;
                    }
                }
            }
            
            if (delegateType == DelegateType.Parent)
            {
                ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.moveHandler);
            }
            else if (delegateType == DelegateType.Object)
            {
                ExecuteEvents.ExecuteHierarchy(delegateObject, eventData, ExecuteEvents.moveHandler);
            }
        }
    }
}