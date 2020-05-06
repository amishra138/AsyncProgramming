# Asynchronous Programming in .NET

Asynchrony is when work is being executed on a different thread and does not impact the main thread of the application

- async and await keywords are contextual keywords that should always be used together

- Traditional - Threading(Low-Level) - Background worker (Event based asynchronous pattern)
- Current - Task parallel library - Async and await

- Async methods are intended to be non-blocking operations
- An await expression in an async method doesn't block the current thread while the awaited task is running.Instead, the expression signs up the rest of the method as a continuation and returns control to the caller of the async method

- Async methods don't require multithreading because an async method doesn't run on its own thread. The method runs on the current synchronization context and uses time on the thread only when the method is active

- You can use Task.Run to move CPU-bound work to a background thread, but a background thread doesn't help with a process that's just waiting for results to become available.

- In combination with the Task.Run method, async programming is better than BackgroundWorker for CPU-bound operations because async programming separates the coordination details of running your code from the work that Task.Run transfers to the threadpool.
