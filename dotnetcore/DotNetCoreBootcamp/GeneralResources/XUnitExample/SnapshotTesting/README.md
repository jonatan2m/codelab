Snapshot testing is a way to validate complex data models and documents.
It generate a file, with the name following the test's name. On the next execution, the tool will compare the lasted file against the current file.
If everything is ok, the test passes, otherwise, it'll fail.

The first result will be versioned and all other execution must to fit with the previous result.

Here is a reference to this test: https://github.com/VerifyTests/Verify
Youtube video that I discovered it https://www.youtube.com/watch?v=Q1_YkcPwpqY

Snapshot tests never assert the correct behavior of the application functionality but does an output comparison instead.

- https://jestjs.io/docs/snapshot-testing
- https://circleci.com/blog/snapshot-testing-with-jest/#:~:text=Snapshot%20testing%20is%20a%20type,values%20of%20your%20development%20team.