using System;
using System.Collections.Generic;
namespace RevolutionaryHostRoles
{

    //town of hostより！！ありがとうございます！！！！！
    class LateTask
    {
        public string name;
        public float timer;
        public Action action;
        public static List<LateTask> Tasks = new();
        public bool Run(float deltaTime)
        {
            timer -= deltaTime;
            if (timer <= 0)
            {
                action();
                return true;
            }
            return false;
        }
        public LateTask(Action action, float time, string name = "No Name Task")
        {
            this.action = action;
            this.timer = time;
            this.name = name;
            Tasks.Add(this);
            RevolutionaryHostRolesPlugin.Logger.LogInfo("\"" + name + "\" is created" + "LateTask");
        }
        public static void Update(float deltaTime)
        {
            var TasksToRemove = new List<LateTask>();
            for (int i = 0; i < Tasks.Count; i++)
            {
                var task = Tasks[i];
                try
                {
                    if (task.Run(deltaTime))
                    {
                        RevolutionaryHostRolesPlugin.Logger.LogInfo($"\"{task.name}\" is finished + LateTask");
                        TasksToRemove.Add(task);
                    }
                }
                catch (Exception ex)
                {
                    RevolutionaryHostRolesPlugin.Logger.LogError($"{ex.GetType()}: {ex.Message}  in \"{task.name}\"\n{ex.StackTrace}" + "LateTask.Error");
                    TasksToRemove.Add(task);
                }
            }
            TasksToRemove.ForEach(task => Tasks.Remove(task));
        }
    }
}