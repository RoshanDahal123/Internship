import { useEffect, useState } from "react";
import { Eye, Loader2, Pencil, Trash2, UserPlus } from "lucide-react";
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
import {
  useDeleteStudentEntryMutation,
  useGetStudentsInfiniteQuery,
} from "../../features/studentApiSlice";

const PAGE_SIZE = 10;
// Safety cap on how many sequential page-fetches we'll chain through when
// jumping to a page we haven't cached yet (e.g. someone lands on ?page=8
// directly, or clicks a page number far past what's loaded).
const MAX_CHAINED_FETCHES = 50;

export default function StudentList() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();

  const urlPage = Number(searchParams.get("page") ?? "1");

  const [deleteStudentEntry, { isLoading: isDeleting }] =
    useDeleteStudentEntryMutation();

  // useGetStudentsInfiniteQuery() has no query arg (QueryArg is `void` in
  // the endpoint definition) — pagination is driven entirely through
  // fetchNextPage()/fetchPreviousPage() and the pageParam (page number).
  const {
    data,
    isLoading,
    isError,
    fetchNextPage,
    hasNextPage,
  } = useGetStudentsInfiniteQuery();

  // pageIndex is zero-based and indexes straight into data.pages[pageIndex].
  // RTK Query's infinite query keeps every page it has fetched, in order, in
  // that array — moving to an already-cached page is just re-pointing this
  // index, no network call. Moving to a page we haven't fetched yet chains
  // fetchNextPage() calls until we reach it (see the effect below).
  const [pageIndex, setPageIndex] = useState(Math.max(urlPage - 1, 0));

  // Keep pageIndex in sync whenever the URL's page changes (pagination
  // clicks, typing a URL, or browser back/forward).
  useEffect(() => {
    setPageIndex(Math.max(urlPage - 1, 0));
  }, [urlPage]);

  // Whenever the target pageIndex isn't cached yet, chain fetchNextPage()
  // calls forward until we reach it (or run out of pages). This is the
  // trade-off of infinite-query-style pagination: you can't jump straight
  // to an arbitrary page, you fetch your way there page by page.
  useEffect(() => {
    let cancelled = false;

    const ensurePageLoaded = async () => {
      let cachedCount = data?.pages.length ?? 0;
      let canFetchMore = hasNextPage;
      let attempts = 0;

      while (
        !cancelled &&
        cachedCount <= pageIndex &&
        canFetchMore &&
        attempts < MAX_CHAINED_FETCHES
      ) {
        const result = await fetchNextPage();
        const newCachedCount = result.data?.pages.length ?? cachedCount;
        canFetchMore = result.hasNextPage ?? false;
        if (newCachedCount === cachedCount && !canFetchMore) break;
        cachedCount = newCachedCount;
        attempts += 1;
      }
    };

    ensurePageLoaded();
    return () => {
      cancelled = true;
    };
    // Deliberately only re-runs when the target page changes.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pageIndex]);

  const currentPage = data?.pages[pageIndex];
  const items = currentPage?.items ?? [];
  const totalCount = currentPage?.totalCount ?? 0;

  // Still "loading" if the very first request hasn't resolved, or if the
  // page the user asked for isn't in the cache yet.
  const isFetching = isLoading || (!currentPage && !isError);

  const handlePageChange = (newPage: number) => {
    setSearchParams({ page: newPage.toString() });
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteStudentEntry(id).unwrap();
      toast.success("Student deleted successfully");
    } catch {
      toast.error("Failed to delete student");
    }
  };

  // Calculate scope numbers for footer metadata
  const startRange = totalCount === 0 ? 0 : pageIndex * PAGE_SIZE + 1;
  const endRange = Math.min((pageIndex + 1) * PAGE_SIZE, totalCount);

  return (
    <TooltipProvider>
      <div className="max-w-5xl mx-auto p-4 sm:p-6 space-y-6">
        {/* Header Section */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-2xl font-semibold tracking-tight">Students</h1>
            <p className="text-sm text-muted-foreground">
              Manage student profiles, view information, and perform operations.
            </p>
          </div>
          <Button onClick={() => navigate("/students/new")} className="gap-2">
            <UserPlus className="h-4 w-4" />
            Add Student
          </Button>
        </div>

        {/* Main Content Container */}
        <Card className="shadow-xs border-border/60">
          <CardHeader className="p-4 sm:p-6 pb-4 flex-row items-center justify-between border-b border-border/40 space-y-0">
            <div>
              <CardTitle className="text-base font-medium">Directory</CardTitle>
              <CardDescription className="text-xs">
                {totalCount > 0 ? `${totalCount} registered students` : "No data available"}
              </CardDescription>
            </div>
          </CardHeader>

          <CardContent className="p-0">
            {/* Loading Skeleton */}
            {isFetching && (
              <div className="p-4 space-y-3">
                {Array.from({ length: 5 }).map((_, i) => (
                  <Skeleton key={i} className="h-12 w-full rounded-md" />
                ))}
              </div>
            )}

            {/* Error State */}
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

            {/* Empty State */}
            {!isFetching && !isError && items.length === 0 && (
              <div className="text-center py-12 px-4 space-y-3">
                <p className="text-sm font-medium text-muted-foreground">
                  No students found
                </p>
                <p className="text-xs text-muted-foreground/80 max-w-sm mx-auto">
                  Get started by creating a new student record using the button above.
                </p>
              </div>
            )}

            {/* Table Rendering */}
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
                    <TableRow key={student.id} className="transition-colors hover:bg-muted/30">
                      <TableCell className="font-medium text-foreground py-3">
                        {student.name}
                      </TableCell>
                      <TableCell className="text-muted-foreground py-3">
                        {student.email}
                      </TableCell>
                      <TableCell className="text-right py-3 pr-4">
                        <div className="flex items-center justify-end gap-1">
                          {/* View Action */}
                          <Tooltip>
                            <TooltipTrigger>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                onClick={() => navigate(`/students/${student.id}`)}
                              >
                                <Eye className="h-4 w-4" />
                                <span className="sr-only">View Details</span>
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>View details</TooltipContent>
                          </Tooltip>

                          {/* Edit Action */}
                          <Tooltip>
                            <TooltipTrigger>
                              <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-muted-foreground hover:text-foreground"
                                onClick={() => navigate(`/students/${student.id}/edit`)}
                              >
                                <Pencil className="h-4 w-4" />
                                <span className="sr-only">Edit</span>
                              </Button>
                            </TooltipTrigger>
                            <TooltipContent>Edit record</TooltipContent>
                          </Tooltip>

                          {/* Delete Modal */}
                          <AlertDialog>
                            <Tooltip>
                              <TooltipTrigger>
                                <AlertDialogTrigger>
                                  <Button
                                    variant="ghost"
                                    size="icon"
                                    className="h-8 w-8 text-muted-foreground hover:text-destructive hover:bg-destructive/10"
                                  >
                                    <Trash2 className="h-4 w-4" />
                                    <span className="sr-only">Delete</span>
                                  </Button>
                                </AlertDialogTrigger>
                              </TooltipTrigger>
                              <TooltipContent>Delete record</TooltipContent>
                            </Tooltip>

                            <AlertDialogContent>
                              <AlertDialogHeader>
                                <AlertDialogTitle>
                                  Delete {student.name}?
                                </AlertDialogTitle>
                                <AlertDialogDescription>
                                  This action cannot be undone. This will permanently remove the student record and associated data.
                                </AlertDialogDescription>
                              </AlertDialogHeader>
                              <AlertDialogFooter>
                                <AlertDialogCancel disabled={isDeleting}>
                                  Cancel
                                </AlertDialogCancel>
                                <AlertDialogAction
                                  variant="destructive"
                                  onClick={(e) => {
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
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            )}
          </CardContent>

          {/* Card Footer with Pagination & Range Data */}
          {!isFetching && !isError && items.length > 0 && (
            <div className="p-4 border-t border-border/40 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
              <p className="text-xs text-muted-foreground text-center sm:text-left">
                Showing <span className="font-medium text-foreground">{startRange}</span> to{" "}
                <span className="font-medium text-foreground">{endRange}</span> of{" "}
                <span className="font-medium text-foreground">{totalCount}</span> results
              </p>
              <StudentPagination
                page={pageIndex + 1}
                pageSize={PAGE_SIZE}
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