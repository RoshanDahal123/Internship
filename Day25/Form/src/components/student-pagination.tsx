import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "../components/ui/pagination";

interface StudentPaginationProps {
  page: number;
  pageSize: number;
  totalCount: number;
  onPageChange: (page: number) => void;
  siblingCount?: number; // Number of pages to show around current page
}

export function StudentPagination({
  page,
  pageSize,
  totalCount,
  onPageChange,
  siblingCount = 1,
}: StudentPaginationProps) {
  const totalPages = Math.ceil(totalCount / pageSize);

  if (totalPages <= 1) {
    return null;
  }

  // Helper to generate the pagination array containing page numbers and '...' strings
  const getPageNumbers = () => {
    const totalNumbers = siblingCount * 2 + 5; // First, last, current, + 2 ellipses

    // Case 1: If total pages is less than the page items we want to show
    if (totalPages <= totalNumbers) {
      return Array.from({ length: totalPages }, (_, i) => i + 1);
    }

    const leftSiblingIndex = Math.max(page - siblingCount, 1);
    const rightSiblingIndex = Math.min(page + siblingCount, totalPages);

    const shouldShowLeftDots = leftSiblingIndex > 2;
    const shouldShowRightDots = rightSiblingIndex < totalPages - 1;

    // Case 2: No left dots, but right dots needed
    if (!shouldShowLeftDots && shouldShowRightDots) {
      const leftItemCount = 3 + 2 * siblingCount;
      const leftRange = Array.from({ length: leftItemCount }, (_, i) => i + 1);
      return [...leftRange, "...", totalPages];
    }

    // Case 3: No right dots, but left dots needed
    if (shouldShowLeftDots && !shouldShowRightDots) {
      const rightItemCount = 3 + 2 * siblingCount;
      const rightRange = Array.from(
        { length: rightItemCount },
        (_, i) => totalPages - rightItemCount + i + 1
      );
      return [1, "...", ...rightRange];
    }

    // Case 4: Both left and right dots needed
    const middleRange = Array.from(
      { length: rightSiblingIndex - leftSiblingIndex + 1 },
      (_, i) => leftSiblingIndex + i
    );
    return [1, "...", ...middleRange, "...", totalPages];
  };

  const pages = getPageNumbers();

  return (
    <Pagination>
      <PaginationContent>
        {/* Previous Button */}
        <PaginationItem>
          <PaginationPrevious
            onClick={(event) => {
              event.preventDefault();
              if (page > 1) onPageChange(page - 1);
            }}
            className={
              page === 1 ? "pointer-events-none opacity-50" : "cursor-pointer"
            }
          />
        </PaginationItem>

        {/* Page items and Ellipses */}
        {pages.map((pageNumber, index) => {
          if (pageNumber === "...") {
            return (
              <PaginationItem key={`ellipsis-${index}`}>
                <PaginationEllipsis />
              </PaginationItem>
            );
          }

          return (
            <PaginationItem key={pageNumber}>
              <PaginationLink
                isActive={pageNumber === page}
                onClick={(event) => {
                  event.preventDefault();
                  onPageChange(pageNumber as number);
                }}
              >
                {pageNumber}
              </PaginationLink>
            </PaginationItem>
          );
        })}

        {/* Next Button */}
        <PaginationItem>
          <PaginationNext
            onClick={(event) => {
              event.preventDefault();
              if (page < totalPages) onPageChange(page + 1);
            }}
            className={
              page === totalPages
                ? "pointer-events-none opacity-50"
                : "cursor-pointer"
            }
          />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  );
}