"use client";

import {
  faCheck,
  faPlus,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import {
  useAddTodoMutation,
  useDeleteTodoMutation,
  useGetTodosQuery,
  useUpdateTodoMutation,
} from "./api/apiSlice";

const TodoList = () => {
  const [newTodo, setNewTodo] = useState("");

  const {
    data: todos = [],
    isLoading,
    isSuccess,
    isError,
    error,
  } = useGetTodosQuery();

  const [addTodo, { isLoading: isAdding }] = useAddTodoMutation();
  const [updateTodo] = useUpdateTodoMutation();
  const [deleteTodo] = useDeleteTodoMutation();

  const handleSubmit = async (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    const title = newTodo.trim();

    if (!title) return;

    try {
      await addTodo({
        userId: 1,
        title,
        completed: false,
      }).unwrap();

      setNewTodo("");
    } catch (error) {
      console.error("Failed to add todo:", error);
    }
  };

  const handleToggle = async (
    id: string,
    completed: boolean,
    userId: number,
    title: string
  ) => {
    try {
      await updateTodo({
        id,
        userId,
        title,
        completed: !completed,
      }).unwrap();
    } catch (error) {
      console.error("Failed to update todo:", error);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteTodo({ id }).unwrap();
    } catch (error) {
      console.error("Failed to delete todo:", error);
    }
  };

  let content;

  if (isLoading) {
    content = (
      <div className="flex justify-center py-10">
        <div className="h-8 w-8 animate-spin rounded-full border-4 border-gray-200 border-t-blue-600" />
      </div>
    );
  } else if (isError) {
    content = (
      <div className="rounded-xl border border-red-200 bg-red-50 p-5 text-center">
        <p className="font-medium text-red-700">
          Failed to load todos.
        </p>
        <p className="mt-1 text-sm text-red-500">
          Please try again later.
        </p>
      </div>
    );
  } else if (isSuccess && todos.length === 0) {
    content = (
      <div className="rounded-xl border border-dashed border-gray-300 bg-white p-10 text-center">
        <p className="text-lg font-medium text-gray-600">
          No todos yet
        </p>
        <p className="mt-1 text-sm text-gray-400">
          Add your first task above.
        </p>
      </div>
    );
  } else {
    content = (
      <div className="space-y-3">
        {todos.map((todo) => (
          <article
            key={todo.id}
            className="flex items-center justify-between rounded-xl border border-gray-200 bg-white p-4 shadow-sm transition hover:shadow-md"
          >
            <div className="flex min-w-0 items-center gap-3">
              <input
                type="checkbox"
                id={`todo-${todo.id}`}
                checked={todo.completed}
                onChange={() =>
                  handleToggle(
                    todo.id,
                    todo.completed,
                    todo.userId,
                    todo.title
                  )
                }
                className="h-5 w-5 cursor-pointer rounded border-gray-300 text-blue-600 focus:ring-blue-500"
              />

              <label
                htmlFor={`todo-${todo.id}`}
                className={`cursor-pointer truncate text-base ${
                  todo.completed
                    ? "text-gray-400 line-through"
                    : "text-gray-700"
                }`}
              >
                {todo.title}
              </label>
            </div>

            <button
              type="button"
              onClick={() => handleDelete(todo.id)}
              className="ml-4 flex h-9 w-9 shrink-0 items-center justify-center rounded-lg text-gray-400 transition hover:bg-red-50 hover:text-red-500"
              aria-label={`Delete ${todo.title}`}
            >
              <FontAwesomeIcon icon={faTrash} />
            </button>
          </article>
        ))}
      </div>
    );
  }

  return (
    <main className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 px-4 py-10">
      <div className="mx-auto w-full max-w-2xl">
        {/* Header */}
        <div className="mb-8 text-center">
          <div className="mx-auto mb-3 flex h-14 w-14 items-center justify-center rounded-2xl bg-blue-600 text-white shadow-lg">
            <FontAwesomeIcon icon={faCheck} className="text-xl" />
          </div>

          <h1 className="text-4xl font-bold tracking-tight text-gray-900">
            Todo List
          </h1>

          <p className="mt-2 text-gray-500">
            Keep track of your tasks and get things done.
          </p>
        </div>

        {/* Add Todo */}
        <form
          onSubmit={handleSubmit}
          className="mb-6 rounded-2xl border border-gray-200 bg-white p-5 shadow-sm"
        >
          <label
            htmlFor="new-todo"
            className="mb-2 block text-sm font-semibold text-gray-700"
          >
            Add a new task
          </label>

          <div className="flex gap-2">
            <input
              type="text"
              id="new-todo"
              value={newTodo}
              onChange={(e) => setNewTodo(e.target.value)}
              placeholder="What needs to be done?"
              className="min-w-0 flex-1 rounded-xl border border-gray-300 px-4 py-3 text-gray-800 outline-none transition placeholder:text-gray-400 focus:border-blue-500 focus:ring-4 focus:ring-blue-100"
            />

            <button
              type="submit"
              disabled={isAdding || !newTodo.trim()}
              className="flex shrink-0 items-center gap-2 rounded-xl bg-blue-600 px-5 py-3 font-medium text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-50"
            >
              <FontAwesomeIcon icon={faPlus} />
              <span>Add</span>
            </button>
          </div>
        </form>

        {/* Todo Stats */}
        {isSuccess && todos.length > 0 && (
          <div className="mb-4 flex items-center justify-between px-1">
            <p className="text-sm text-gray-500">
              {todos.length}{" "}
              {todos.length === 1 ? "task" : "tasks"}
            </p>

            <p className="text-sm font-medium text-blue-600">
              {todos.filter((todo) => todo.completed).length} completed
            </p>
          </div>
        )}

        {/* Todos */}
        {content}
      </div>
    </main>
  );
};

export default TodoList;