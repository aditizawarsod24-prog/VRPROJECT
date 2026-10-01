using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ChildSequenceController : MonoBehaviour
{
    [Tooltip("Child transforms in the order they should be shown")]
    [SerializeField] private List<Transform> children = new List<Transform>();

    [Tooltip("Fired when ShowNext() is called while already on the last child")]
    public UnityEvent onLastChildReached;

    private int currentIndex = 0;

    private void Awake()
    {
        // Auto-populate from actual children if the list wasn't set manually
        if (children.Count == 0)
        {
            foreach (Transform child in transform)
            {
                children.Add(child);
            }
        }
    }

    private void Start()
    {
        ShowOnlyIndex(0);
    }

    [ContextMenu("Show Next")]
    public void ShowNext()
    {
        if (children.Count == 0) return;

        if (currentIndex >= children.Count - 1)
        {
            onLastChildReached.Invoke();
            return;
        }

        currentIndex++;
        ShowOnlyIndex(currentIndex);
    }

    /// <summary>
    /// Optional: go back to the previous child.
    /// </summary>
    public void ShowPrevious()
    {
        if (children.Count == 0 || currentIndex <= 0) return;

        currentIndex--;
        ShowOnlyIndex(currentIndex);
    }

    /// <summary>
    /// Optional: reset back to the first child.
    /// </summary>
    public void ResetToFirst()
    {
        ShowOnlyIndex(0);
    }

    private void ShowOnlyIndex(int index)
    {
        currentIndex = index;
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] != null)
                children[i].gameObject.SetActive(i == index);
        }
    }
}