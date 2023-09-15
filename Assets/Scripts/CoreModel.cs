using System;

public class CoreModel
{
    public int taskFoodCount;
    public int taskFoodId;
    
    public CoreModel(int count, int id)
    {
        taskFoodCount = count;
        taskFoodId = id;
    }

    public void ChangeCount(int id)
    {
        if (id != taskFoodId) return;
        taskFoodCount--;
    }
}