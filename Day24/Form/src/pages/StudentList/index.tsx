// src/pages/StudentList/index.tsx
import { Eye, Loader2, Pencil, Search, Trash2, UserPlus } from "lucide-react";
import { useState } from "react";
import { useNavigate, useSearchParams } from "react-router";
import { toast } from "sonner";
import { StudentPagination } from "../../components/student-pagination";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "../../components/ui/alert-dialog";
import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Skeleton } from "../../components/ui/skeleton";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../../components/ui/table";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "../../components/ui/tooltip";
import { selectIsAdmin } from "../../features/authSlice";
import {
  useDeleteStudentEntryMutation,
  useGetStudentsQuery,
} from "../../features/studentApiSlice";
import { useAppSelector } from "../../hooks/reducer-hook";
import { useDebouncedCallback } from "../../hooks/use-debounced-callback";

export default function StudentList() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();

  const isAdmin = useAppSelector(selectIsAdmin);

  const page = Number(searchParams.get("page") ?? "1");
  const urlSearch = searchParams.get("search") ?? "";
  const pageSize = 10;

  // Local input state so typing feels instant; the URL (and thus the
  // actual query) updates on a debounce so we don't spam the API on
  // every keystroke.
  const [searchInput, setSearchInput] = useState(urlSearch);

  const debouncedUpdateUrl = useDebouncedCallback((value: string) => {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      if (value.trim()) {
        next.set("search", value.trim());
      } else {
        next.delete("search");
      }
      next.set("page", "1"); // new search -> always restart at page 1
      return next;
    });
  }, 300);

  function handleSearchChange(e: React.ChangeEvent<HTMLInputElement>) {
    setSearchInput(e.target.value);
    debouncedUpdateUrl(e.target.value);
  }

  const [deleteStudentEntry, { isLoading: isDeleting }] =
    useDeleteStudentEntryMutation();
  const { data, isLoading: isFetching, isError } = useGetStudentsQuery({
    page,
    search: urlSearch,
  });

  const totalCount = data?.totalCount ?? 0;
  const items = data?.items ?? [];

  const handlePageChange = (newPage: number) => {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      next.set("page", newPage.toString());
      return next;
    });
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteStudentEntry(id).unwrap();
      toast.success("Student deleted successfully");
    } catch {
      toast.error("Failed to delete student");
    }
  };

  const startRange = totalCount === 0 ? 0 : (page - 1) * pageSize + 1;
  const endRange = Math.min(page * pageSize, totalCount);

  return (
    <TooltipProvider>
      <div className="max-w-5xl mx-auto p-4 sm:p-6 space-y-6">
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-2xl font-semibold tracking-tight">Students</h1>
            <p className="text-sm text-muted-foreground">
              {isAdmin
                ? "Manage student profiles, view information, and perform operations."
                : "Browse the student directory."}
            </p>
          </div>
          {isAdmin && (
            <Button onClick={() => navigate("/students/new")} className="gap-2">
              <UserPlus className="h-4 w-4" />
              Add Student
            </Button>
          )}
        </div>

        <Card className="shadow-xs border-border/60">
          <CardHeader className="p-4 sm:p-6 pb-4 flex flex-col gap-4 border-b border-border/40 sm:flex-row sm:items-center sm:justify-between space-y-0">
            <div>
              <CardTitle className="text-base font-medium">Directory</CardTitle>
              <CardDescription className="text-xs">
                {totalCount > 0
                  ? `${totalCount} registered student${totalCount === 1 ? "" : "s"}`
                  : "No data available"}
              </CardDescription>
            </div>
            <div className="relative w-full sm:max-w-xs">
              <Search className="absolute left-2.5 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
              <Input
                value={searchInput}
                onChange={handleSearchChange}
                placeholder="Search students..."
                className="h-9 pl-8"
                aria-label="Search students"
              />
            </div>
          </CardHeader>

          <CardContent className="p-0">
            {isFetching && (
              <div className="p-4 space-y-3">
                {Array.from({ length: 5 }).map((_, i) => (
                  <Skeleton key={i} className="h-12 w-full rounded-md" />
                ))}
              </div>
            )}

            {isError && (
              <div className="p-8 text-center space-y-2">
                <p className="text-sm font-medium text-destructive">
                  Failed to load students
                </p>
                <p className="text-xs text-muted-foreground">
                  There was a problem retrieving data from the server.
                </p>
              </div>
            )}

            {!isFetching && !isError && items.length === 0 && (
              <div className="text-center py-12 px-4 space-y-3">
                <p className="text-sm font-medium text-muted-foreground">
                  {urlSearch
                    ? `No students match "${urlSearch}"`
                    : "No students found"}
                </p>
                {isAdmin && !urlSearch && (
                  <p className="text-xs text-muted-foreground/80 max-w-sm mx-auto">
                    Get started by creating a new student record using the
                    button above.
                  </p>
                )}
              </div>
            )}

            {!isFetching && !isError && items.length > 0 && (
              <Table>
                <TableHeader className="bg-muted/40">
                  <TableRow className="hover:bg-transparent">
                    <TableHead className="w-[40%] font-medium">Name</TableHead>
                    <TableHead className="w-[40%] font-medium">Email</TableHead>
                    <TableHead className="w-[20%] text-right font-medium pr-6">
                      Actions
                    </TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {items.map((student) => (
                    <TableRow
                      key={student.id}
                      className="transition-colors hover:bg-muted/30"
                    >
                      <TableCell className="font-medium text-foreground py-3">
                        {student.name}
                      </TableCell>
                      <TableCell className="text-muted-foreground py-3">
                        {student.email}
                      </TableCell>
                      <TableCell className="text-right py-3 pr-4">
                        <div className="flex items-center justify-end gap-1">
                          <Tooltip>
                            <TooltipTrigger
                              render={
                                <Button
                                  variant="ghost"
                                  size="icon"
                                  className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                  onClick={() =>
                                    navigate(`/students/${student.id}`)
                                  }
                                >
                                  <Eye className="h-4 w-4" />
                                  <span className="sr-only">View Details</span>
                                </Button>
                              }
                            />
                            <TooltipContent>View details</TooltipContent>
                          </Tooltip>

                          {isAdmin && (
                            <>
                              <Tooltip>
                                <TooltipTrigger
                                  render={
                                    <Button
                                      variant="ghost"
                                      size="icon"
                                      className="h-8 w-8 text-green-400 hover:text-foreground"
                                      onClick={() =>
                                        navigate(`/students/${student.id}/edit`)
                                      }
                                    >
                                      <Pencil className="h-4 w-4" />
                                      <span className="sr-only">Edit</span>
                                    </Button>
                                  }
                                />
                                <TooltipContent>Edit record</TooltipContent>
                              </Tooltip>

                              <AlertDialog>
                                <Tooltip>
                                  <TooltipTrigger
                                    render={
                                      <AlertDialogTrigger
                                        render={
                                          <Button
                                            variant="ghost"
                                            size="icon"
                                            className="h-8 w-8 text-destructive hover:text-destructive hover:bg-destructive/10"
                                          >
                                            <Trash2 className="h-4 w-4" />
                                            <span className="sr-only">
                                              Delete
                                            </span>
                                          </Button>
                                        }
                                      />
                                    }
                                  />
                                  <TooltipContent>Delete record</TooltipContent>
                                </Tooltip>

                                <AlertDialogContent>
                                  <AlertDialogHeader>
                                    <AlertDialogTitle>
                                      Delete {student.name}?
                                    </AlertDialogTitle>
                                    <AlertDialogDescription>
                                      This action cannot be undone. This will
                                      permanently remove the student record and
                                      associated data.
                                    </AlertDialogDescription>
                                  </AlertDialogHeader>
                                  <AlertDialogFooter>
                                    <AlertDialogCancel
                                      variant="outline"
                                      size="default"
                                      disabled={isDeleting}
                                    >
                                      Cancel
                                    </AlertDialogCancel>
                                    <AlertDialogAction
                                      variant="destructive"
                                      onClick={(e: any) => {
                                        e.preventDefault();
                                        handleDelete(student.id);
                                      }}
                                      disabled={isDeleting}
                                    >
                                      {isDeleting && (
                                        <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                                      )}
                                      Delete
                                    </AlertDialogAction>
                                  </AlertDialogFooter>
                                </AlertDialogContent>
                              </AlertDialog>
                            </>
                          )}
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>

          {!isFetching && !isError && items.length > 0 && (
            <div className="p-4 border-t border-border/40 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
              <p className="text-xs text-muted-foreground text-center sm:text-left">
                Showing{" "}
                <span className="font-medium text-foreground">{startRange}</span>{" "}
                to{" "}
                <span className="font-medium text-foreground">{endRange}</span>{" "}
                of{" "}
                <span className="font-medium text-foreground">{totalCount}</span>{" "}
                results
              </p>
              <StudentPagination
                page={page}
                pageSize={pageSize}
                totalCount={totalCount}
                onPageChange={handlePageChange}
              />
            </div>
          )}
        </Card>
      </div>
    </TooltipProvider>
  );
}
