using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.Movement;

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

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastRadius = 0.25f;


        private PlayerSelector selectedPlayer;

        // Start is called before the first frame update
        void Start()
        {
            selectedPlayer = PlayerSelector.GetFirstSelectedPlayer().GetComponent<PlayerSelector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithUI()) return;

            bool playerSelection = false;

            if (Input.GetMouseButtonDown(0))
            {
                 playerSelection = InteractWithPlayerSelection(ControlKeyPressed());
            }
            if (InteractWithComponent()) return;
            if (playerSelection) return;
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

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.collider.gameObject.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(selectedPlayer) != RaycastableReturnValue.NoAction)
                    {
                        SetCursorType(raycastable.GetCursorType());
                        if (Input.GetMouseButtonDown(0))
                        {
                            raycastable.HandleActivation(selectedPlayer);
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
            bool hashit = RaycastNavMesh(out target);
            if (hashit)
            {
                Mover mover = selectedPlayer.GetComponent<Mover>();
                if (!mover.CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    mover.StartMovementAction(target, 1f); ;
                }
                SetCursorType(CursorType.Movement);
                return true;
            }
            return false;
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
            return Camera.main.ScreenPointToRay(Input.mousePosition);
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



        private bool InteractWithPlayerSelection(bool controlKeyPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                if (raycastHit.transform.TryGetComponent<PlayerSelector>(out PlayerSelector playerSelector))
                {
                    if (!controlKeyPressed)
                    {
                        DeSelectOtherPlayerControllers();
                    }
                    selectedPlayer = playerSelector;
                    selectedPlayer.SetSelected(true);
                    return true;

                }
            }

            return false;
        }

        private void DeSelectOtherPlayerControllers()
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in allPlayers)
            {
                player.GetComponent<PlayerSelector>().SetSelected(false);
            }
        }

        private bool ControlKeyPressed()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }
    }

}

