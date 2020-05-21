# Asynchronous Programming in .NET

Asynchrony is when work is being executed on a different thread and does not impact the main thread of the application

- async and await keywords are contextual keywords that should always be used together

- Traditional - Threading(Low-Level) - Background worker (Event based asynchronous pattern)
- Current - Task parallel library - Async and await

- The core of async programming is the Task and Task<T> objects, which model asynchronous operations. They are supported by the async and await keywords. The model is fairly simple in most cases:

1. For I/O-bound code, you await an operation which returns a Task or Task<T> inside of an async method.

2. For CPU-bound code, you await an operation which is started on a background thread with the Task.Run method.

- The await keyword is where the magic happens. It yields control to the caller of the method that performed await, and it ultimately allows a UI to be responsive or a - service to be elastic.

- Async methods are intended to be non-blocking operations
- An await expression in an async method doesn't block the current thread while the awaited task is running.Instead, the expression signs up the rest of the method as a continuation and returns control to the caller of the async method

- Async methods don't require multithreading because an async method doesn't run on its own thread. The method runs on the current synchronization context and uses time on the thread only when the method is active

- You can use Task.Run to move CPU-bound work to a background thread, but a background thread doesn't help with a process that's just waiting for results to become available.

- In combination with the Task.Run method, async programming is better than BackgroundWorker for CPU-bound operations because async programming separates the coordination details of running your code from the work that Task.Run transfers to the threadpool.

## async and await

If you specify that a method is an async method by using the async modifier, you enable the following two capabilities.

- The marked async method can use await to designate suspension points. The await operator tells the compiler that the async method can't continue past that point until the awaited asynchronous process is complete. In the meantime, control returns to the caller of the async method.

The suspension of an async method at an await expression doesn't constitute an exit from the method, and finally blocks don't run.

- The marked async method can itself be awaited by methods that call it.

An async method typically contains one or more occurrences of an await operator, but the absence of await expressions doesn't cause a compiler error. If an async method doesn't use an await operator to mark a suspension point, the method executes as a synchronous method does, despite the async modifier. The compiler issues a warning for such methods.

## Return type and parameters

- A task represents ongoing work. A task encapsulates information about the state of the asynchronous process and, eventually, either the final result from the process or the exception that the process raises if it doesn't succeed.
- An async method can also have a void return type. This return type is used primarily to define event handlers, where a void return type is required. Async event handlers often serve as the starting point for async programs.
- An async method that has a void return type can't be awaited, and the caller of a void-returning method can't catch any exceptions that the method throws.
- An async method can't declare in, ref or out parameters, but the method can call methods that have such parameters. Similarly, an async method can't return a value by reference, although it can call methods with ref return values.

## What are the difference between THREAD, PROCESS and TASK?

A process invokes or initiates a program. It is an instance of a program that can be multiple and running the same application. A thread is the smallest unit of execution that lies within the process. A process can have multiple threads running. An execution of thread results in a task. Hence, in a multithreading environment, multithreading takes place.

A program in execution is known as ‘process’. A program can have any number of processes. Every process has its own address space.

Threads uses address spaces of the process. The difference between a thread and a process is, when the CPU switches from one process to another the current information needs to be saved in Process Descriptor and load the information of a new process. Switching from one thread to another is simple.

A task is simply a set of instructions loaded into the memory. Threads can themselves split themselves into two or more simultaneously running tasks.

## Recognize CPU-Bound and I/O-Bound Work

- If the work you have is I/O-bound, use async and await without Task.Run. You should not use the Task Parallel Library. The reason for this is outlined in the Async in Depth article. (https://docs.microsoft.com/en-us/dotnet/standard/async-in-depth)

- If the work you have is CPU-bound and you care about responsiveness, use async and await but spawn the work off on another thread with Task.Run. If the work is appropriate for concurrency and parallelism, you should also consider using the Task Parallel Library. (https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl)

## Waiting for Multiple Tasks to Complete

The Task API contains two methods, Task.WhenAll and Task.WhenAny which allow you to write asynchronous code which performs a non-blocking wait on multiple background jobs.

Ex:
public async Task<User> GetUserAsync(int userId)
{
// Code omitted:
//
// Given a user Id {userId}, retrieves a User object corresponding
// to the entry in the database with {userId} as its Id.
}

public static async Task<IEnumerable<User>> GetUsersAsync(IEnumerable<int> userIds)
{
var getUserTasks = new List<Task<User>>();

    foreach (int userId in userIds)
    {
        getUserTasks.Add(GetUserAsync(userId));
    }

    return await Task.WhenAll(getUserTasks);

}

Here's another way to write this a bit more succinctly, using LINQ:

public async Task<User> GetUserAsync(int userId)
{
// Code omitted:
//
// Given a user Id {userId}, retrieves a User object corresponding
// to the entry in the database with {userId} as its Id.
}

public static async Task<User[]> GetUsersAsync(IEnumerable<int> userIds)
{
var getUserTasks = userIds.Select(id => GetUserAsync(id));
return await Task.WhenAll(getUserTasks);
}

- Although it's less code, take care when mixing LINQ with asynchronous code. Because LINQ uses deferred (lazy) execution, async calls won't happen immediately as they do in a foreach() loop unless you force the generated sequence to iterate with a call to .ToList() or .ToArray().

- It’s important to reason about tasks as abstractions of work happening asynchronously, and not an abstraction over threading. By default, tasks execute on the current thread and delegate work to the Operating System, as appropriate. Optionally, tasks can be explicitly requested to run on a separate thread via the Task.Run API.

## Write code that awaits Tasks in a non-blocking manner

Blocking the current thread as a means to wait for a Task to complete can result in deadlocks and blocked context threads, and can require significantly more complex error-handling. The following table provides guidance on how to deal with waiting for Tasks in a non-blocking way:

Use this...             Instead of this...          When wishing to do this
----------------------------------------------------------------------------------------------------
await                   Task.Wait or Task.Result    Retrieving the result of a background task
await Task.WhenAny      Task.WaitAny                Waiting for any task to complete
await Task.WhenAll      Task.WaitAll                Waiting for all tasks to complete
await Task.Delay        Thread.Sleep                Waiting for a period of time

## Cancel async tasks after a period of time
You can cancel an asynchronous operation after a period of time by using the CancellationTokenSource.CancelAfter method if you don't want to wait for the operation to finish. This method schedules the cancellation of any associated tasks that aren’t complete within the period of time that’s designated by the CancelAfter expression.
Ex:     // ***Set up the CancellationTokenSource to cancel after 2.5 seconds. (You can adjust the time.)
        cts.CancelAfter(2500);

## Start Multiple Async Tasks and Process Them As They Complete 
By using Task.WhenAny, you can start multiple tasks at the same time and process them one by one as they’re completed rather than process them in the order in which they're started.
