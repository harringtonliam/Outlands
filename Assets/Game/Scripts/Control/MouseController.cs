using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.Movement;
using RPG.Core;
using RPG.UI.InGame;

namespace RPG.Control
{
    public class MouseController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture2D;
            public Vector2 hotspot;
        }

        [SerializeField] private RectTransform selectionBox;
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastRadius = 0.25f;
        [SerializeField] Formation defaultFormation;
        [SerializeField] private Camera camera;


        private PlayerSelector selectedPlayer;
        private bool isMouseBeingDragged = false;
        private Vector2 startingMousePosition;
        HashSet<PlayerSelector> playersToSelect = new(5);


        // Start is called before the first frame update
        void Start()
        {
            selectedPlayer = PlayerSelector.GetFirstSelectedPlayer().GetComponent<PlayerSelector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithUI()&& !isMouseBeingDragged) return;

            //TODO: THis code testing player selection should not be needed anymore, its all handled in InteractWithComponent since 05/05/2025
            //bool playerSelection = false;
            //if (InputManager.Instance.IsMouseButtonDown())
            //{
            //     playerSelection = InteractWithPlayerSelection(ControlKeyPressed());
            //}
            if (InteractWithComponent()) return;
            //if (playerSelection) return;
            
            if (InteractWithMovement()) return;

            SetCursorType(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursorType(CursorType.UI);
                
            }

  
            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool InteractWithPlayerSelection(bool controlKeyPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                if (raycastHit.transform.TryGetComponent<PlayerSelector>(out PlayerSelector playerSelector))
                {
                    selectedPlayer = playerSelector;
                    selectedPlayer.SetSelected(true, controlKeyPressed);
                    return true;
                }
            }

            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();

            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.collider.gameObject.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    var returnValue = raycastable.HandleRaycast(selectedPlayer);
                    if (returnValue != RaycastableReturnValue.NoAction)
                    {
                        SetCursorType(raycastable.GetCursorType());
                        if (InputManager.Instance.IsMouseButtonDown())
                        {
                            if(returnValue == RaycastableReturnValue.FirstPlayerCharacter)
                            {
                                if(hit.transform.TryGetComponent<PlayerSelector>(out PlayerSelector playerSelector))
                                {
                                    selectedPlayer = playerSelector;
                                    selectedPlayer.SetSelected(true, ControlKeyPressed());
                                    return true;
                                }
                                selectedPlayer = PlayerSelector.GetFirstSelectedPlayer().GetComponent<PlayerSelector>();
                                raycastable.HandleActivation(selectedPlayer);
                            }
                            else if(returnValue == RaycastableReturnValue.AllPlayerCharacters)
                            {
                                Debug.Log("InteractWithComponent doing AllPlayerCharacters " + PlayerSelector.GetAllSelectedPlayers().Count);
                                foreach (var player in PlayerSelector.GetAllSelectedPlayers())
                                {
                                    Debug.Log("InteractWithComponent activating " + player.gameObject.name);
                                    var playerSelector = player.GetComponent<PlayerSelector>();
                                    raycastable.HandleActivation(playerSelector);
                                }
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            Vector3[] offsetTargets = new Vector3[5];
            bool hashit = RaycastNavMesh(out target);

            if (hashit)
            {
                Mover mover = PlayerSelector.GetFirstSelectedPlayer().GetComponent<Mover>();
                if (!mover.CanMoveTo(target)) return false;
                if (InputManager.Instance.IsMouseButtonUp())
                {
                    //Disable the selection box 
                    selectionBox.gameObject.SetActive(false);
                    if (isMouseBeingDragged)
                    {

                        //Record which players are in the selection box
                        var selectectBoxBounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);
                        playersToSelect.Clear();
                        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            Vector2 playerPosition = camera.WorldToScreenPoint(player.transform.position);
                            if (selectectBoxBounds.Contains(playerPosition) && player.TryGetComponent<PlayerSelector>(out PlayerSelector selector))
                            {
                                playersToSelect.Add(selector);
                            }
                        }
                        if (playersToSelect.Count > 0)
                        {
                            PlayerSelector.DeselectAllPlayerCharacters();
                            foreach (PlayerSelector player in playersToSelect)
                            {
                                player.SetSelected(true, true);
                            }
                        }
                        isMouseBeingDragged = false;
                    }
                    else
                    {
                        int playerIndex = 0;
                        foreach (var player in PlayerSelector.GetAllSelectedPlayers())
                        {
                            MovementDestinationIndicator destinationIndicator = player.GetComponent<PlayerSelector>().MovementDestinationIndicator;
                            var newDestinaton = destinationIndicator.transform.position;
                            destinationIndicator.transform.position = player.transform.position;
                            destinationIndicator.gameObject.SetActive(false);
                            Mover playerMover = player.GetComponent<Mover>();
                            if (playerMover != null && playerMover.CanMoveTo(newDestinaton))
                            {
                                playerMover.StartMovementAction(newDestinaton, 1f);
                            }
                            playerIndex++;
                        }
                    }
                }
                if (InputManager.Instance.IsMouseButtonDown())
                {
                    isMouseBeingDragged = false;
                    startingMousePosition = InputManager.Instance.GetMouseScreenPosition();
                    offsetTargets = CalculateMovementTargets(target);
                }
                if (InputManager.Instance.IsMouseButtonPressed() && !InputManager.Instance.IsMouseButtonDown() && HasMouseBeenDraggedFarEnough())
                {
                    if (!isMouseBeingDragged)
                    {
                        //Enable the selection box 
                        selectionBox.gameObject.SetActive(true);
                        //Disable the movement destination Indicators
                        foreach (var player in PlayerSelector.GetAllSelectedPlayers())
                        {
                            MovementDestinationIndicator destinationIndicator = player.GetComponent<PlayerSelector>().MovementDestinationIndicator;
                            destinationIndicator.gameObject.SetActive(false);
                        }
                        //Dragging the mouse
                        isMouseBeingDragged = true;
                    }
                    //Resize the selection box
                    Bounds selectectBoxBounds = ResizeSelectionBox();
                }
                SetCursorType(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool HasMouseBeenDraggedFarEnough()
        {
            if (isMouseBeingDragged) return true;
            return Vector2.Distance(startingMousePosition, InputManager.Instance.GetMouseScreenPosition()) > MinMouseDragDistance();
        }

        private Vector3[] CalculateMovementTargets(Vector3 target)
        {
            Vector3[] targets = new Vector3[5];
            int playerIndex = 0;
            foreach (var player in PlayerSelector.GetAllSelectedPlayers())
            {
                Mover playerMover = player.GetComponent<Mover>();
                if (playerMover != null && playerMover.CanMoveTo(target))
                {
                    targets[playerIndex]= CalculateFormationTarget(target, playerIndex);
                    var destinationIndicator = player.GetComponent<PlayerSelector>().MovementDestinationIndicator;
                    if (destinationIndicator != null)
                    {
                        destinationIndicator.transform.position = targets[playerIndex];
                        destinationIndicator.gameObject.SetActive(true);
                    }

                    //playerMover.StartMovementAction(offSetTarget, 1f);
                }
                playerIndex++;
            }
            return targets;
        }

        public Vector3 CalculateFormationTarget(Vector3 target , int playerIndex)
        {
            if (defaultFormation != null)
            {
                return target + defaultFormation.GetFormationOffsetVector3(playerIndex);
            }

            return target;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            target = new Vector3(0, 0, 0);
            bool hashit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hashit) return false;
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 1f, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;

            return true;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            //sort by distance
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            //return sorted array
            return hits;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        }

        private void SetCursorType(CursorType cursorType)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.texture2D, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (var cursorMapping in cursorMappings)
            {
                if (cursorMapping.cursorType == cursorType)
                {
                    return cursorMapping;
                }
            }
            return new CursorMapping();
        }





        private bool ControlKeyPressed()
        {
            return InputManager.Instance.IsKey(KeyCode.LeftControl) || InputManager.Instance.IsKey(KeyCode.RightControl);
        }

        private float MinMouseDragDistance()
        {
            float minDimension = Mathf.Min(Screen.width, Screen.height);
            return 0.05f * minDimension;
        }

        private Bounds ResizeSelectionBox()
        {
            Vector2 mousePosition = InputManager.Instance.GetMouseScreenPosition();

            float width = mousePosition.x - startingMousePosition.x;
            float height = mousePosition.y - startingMousePosition.y;

            selectionBox.anchoredPosition = startingMousePosition + new Vector2(width / 2, height / 2);
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

            return new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);
        }
    }

}

