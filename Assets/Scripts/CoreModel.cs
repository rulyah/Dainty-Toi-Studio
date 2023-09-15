using System;

public class CoreModel
{
    public int taskFoodCount;
    public int taskFoodId;
    
    public event Action onTaskComplete;

    public CoreModel(int count, int id)
    {
        taskFoodCount = count;
        taskFoodId = id;
    }

    public void ChangeCount(int id)
    {
        if (id != taskFoodId) return;
        taskFoodCount--;
        if(taskFoodCount <= 0) onTaskComplete?.Invoke();
    }
}