using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvasManager : MonoBehaviour
{
    public List<UICanvasItem> uICanvasItems;

    [Header("Initialize")]
    public bool openCanvasAtStart;
    public int canvasItemAtStart = 0;
    
    private UICanvasItem currentUICanvasItem;

    private void Start()
    {
        Close();
        if (openCanvasAtStart) OpenCanvas(canvasItemAtStart);
    }

    public void OpenCanvas(string _canvasName)
    {
        if (currentUICanvasItem != null)
        {
            currentUICanvasItem.CloseCanvas();
        }

        currentUICanvasItem = uICanvasItems.Find(x => x.canvasName == _canvasName);
        currentUICanvasItem.OpenCanvas();
    }

    public void OpenCanvas(int _canvasNum)
    {
        if (_canvasNum >= uICanvasItems.Count)
        {
            Debug.LogError("Input Number is greater than uICanvasItems. Aborted.");
            return;
        }

        if (currentUICanvasItem != null)
        {
            currentUICanvasItem.CloseCanvas();
        }

        currentUICanvasItem = uICanvasItems[_canvasNum];
        currentUICanvasItem.OpenCanvas();
    }

    public void Close()
    {
        foreach (UICanvasItem uICanvasItem in uICanvasItems)
        {
            uICanvasItem.CloseCanvas();
        }

        currentUICanvasItem = null;
    }
}
