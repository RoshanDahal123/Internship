## Basic Git Commands

#### Set your user name and email address
````bash
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"
````
### Starting a repository

````bash
git init     # turns the current folder into a Git repo
git clone <repository-url>  ## turns the current folder into a Git repo
````


1.Check what's changed
````bash
git status   # shows the status of files in the working directory and staging area
````

2. Stage changes (move them to the "waiting room"):
````bash
git add TodoService.cs        # stage one specific file
git add .                      # stage everything changed in the current folder
````

####3. Commit (save a snapshot with a message):
````bash
git commit -m "fix: corrected the logic in TodoService.cs"  # commit staged changes with a message
````
This is exactly the kind of message we wrote together for your README changes — clear, prefixed by type (fix:, docs:, feat:), describing what changed.

4. Push (send your commits to GitHub):
````bash
git push origin main

origin = the remote's name, main = the branch you're pushing to.
````
#### 5. Pull (bring down changes others made on GitHub):

````bash 
git pull origin main

//This is fetch (download the latest changes) + merge (combine them into your current work) done in one step.
````
````bash
git log
git diff     # see exact line by line changes not yet staged
  git branch  # see all branches in the repo
  git checkout <branch-name>  # switch to a different branch
  git checkout -b <new-branch-name>  # create a new branch and switch to it
  git create branch <new-branch-name>  # create a new branch without switching to it
  git switch -c my-new-branch  # create a new branch and switch to it
  git checkout -b my-new-branch  # create a new branch and switch to it
````

   #### Part 3 Merging and resolving conflicts

   ```bash
   git merge <branch-name>  # merge changes from another branch into the current branch
   ```
   This takes the commits from <branch-name> and applies them into your branch.
   If both branches changed the same lines of code, Git will stop and ask you to resolve the conflicts before you can finish the merge.And
   git marks the file with the conflicts markers, and git automatically decide which version to keep, and which version to discard. You can also manually edit the file to resolve the conflicts.


  ````bash
  <<<<<<< HEAD
existing.Priority = todo.Priority;
=======
existing.Priority = newTodo.Priority;
>>>>>>> feature-branch

````


##Cleaning up your commit history

````bash
git checkout main
git pull origin main                    # get the newly merged code
git branch -d feature/add-due-date       # delete the local branch,longer needed

````