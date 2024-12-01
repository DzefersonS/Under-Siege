# Under Siege

## Branch workflow

- Main branch - production branch. Only working, nonbuggy dev branch should be merged into main branch (with a Merge Request). Otherwise, commits should not be made directly to main (unless modifying something that is not code related, like a readme).

- Dev branch - the branch used to test out and add new features in order to seperate stable production branch from potentially buggy dev branch.

When working on a feature, a new branch should be made that splits off of Dev, and named appropriately:
- feature/EnemyHealthbar (as an example)

And once it is done, a merge request should be made into Dev, which can then be reviewed by someone else to spot out preliminary bugs and issues, and when in Dev branch it can be tested out in depth.

Whenever Dev branch is updated, feature branches should synchronize with Dev to catch merge conflicts and other issues early.

This workflows allows for optimal asynchronous work on features while preventing broken branches/builds, and not having to clean up ugly git messes.