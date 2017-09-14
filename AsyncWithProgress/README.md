This is my first crack at responsive GUI using the Task-based Async Pattern (TAP).
The counter method accept a cancellation token, which is created by clicking the Abort button.

To prevent the debugger from breaking, you must go to Exception Settings, clear the CLR exceptions checkbox, then right click CLR
exceptions and check "Continue When Unhandled in User Code."  A properly functioning cancellation handler is meant to throw an error.
If it does not, the task status is always shown as Completed, whether or not it actually ran to completion.
