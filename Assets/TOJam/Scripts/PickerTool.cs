using UnityEngine;

public class PickerTool : MonoBehaviour
{
    public LayerMask validHitMask;
    public Transform holder;
    public float minimumDistance = 0.1f;
    public float attractionPower = 10;
    
    public PuzzlePiece currentPuzzlePiece;
    public PuzzlePiece hoveringPuzzlePiece;

    private IHoverable hoverable;
    
    private void Update()
    {
        CheckHoveringOverPiece();

        if (!currentPuzzlePiece) return;

        var puzzlePiecePosition = currentPuzzlePiece.transform.position;
        var distance = Vector3.Distance(puzzlePiecePosition, holder.position);
        if (distance <= minimumDistance) return;
        
        puzzlePiecePosition = Vector3.MoveTowards(puzzlePiecePosition, holder.position, attractionPower * Time.deltaTime);
        currentPuzzlePiece.transform.position = puzzlePiecePosition;
    }

    private void CheckHoveringOverPiece()
    {
        var ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, validHitMask))
        {
            hoverable = hit.collider.GetComponentInParent<IHoverable>();
            hoverable?.HoverEnter();

            if (currentPuzzlePiece)
            {
                hoveringPuzzlePiece = null;
                return;
            }
            
            var puzzlePiece = hit.collider.GetComponentInParent<PuzzlePiece>();
            if (puzzlePiece) hoveringPuzzlePiece = puzzlePiece;

            return;
        }

        hoverable?.HoverExit();
        hoverable = null;
    }

    public void PickUp()
    {
        if (currentPuzzlePiece)
        {
            currentPuzzlePiece.GotDropped();
            currentPuzzlePiece = null;
        } 
        
        if (!hoveringPuzzlePiece) return;
        currentPuzzlePiece = hoveringPuzzlePiece;
        currentPuzzlePiece.GotPickedUp(holder);
    }
}
