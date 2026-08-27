"use client";

import {
  ArrowLeft,
  Calendar,
  FileText,
  GraduationCap,
  Mail,
  MapPin,
  Pencil,
  User,
} from "lucide-react";
import { Link, useNavigate, useParams } from "react-router";

import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Separator } from "../../components/ui/separator";
import { Skeleton } from "../../components/ui/skeleton";
import { useGetStudentByIdQuery } from "../../features/studentApiSlice";
import DetailItem from "../../components/detail-item";
import { useAppSelector } from "../../hooks/reducer-hook";
import { selectIsAdmin } from "../../features/authSlice";

export default function StudentDetails() {
  const { id } = useParams<{ id: string }>();
  const isAdmin = useAppSelector(selectIsAdmin);
  const navigate = useNavigate();

  const {
    data: student,
    isLoading,
    isError,
  } = useGetStudentByIdQuery(Number(id));

  if (isLoading) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center p-4">
        <Card className="w-full sm:max-w-xl">
          <CardHeader>
            <Skeleton className="h-7 w-40" />
          </CardHeader>
          <CardContent className="space-y-4">
            {Array.from({ length: 4 }).map((_, i) => (
              <Skeleton key={i} className="h-10 w-full" />
            ))}
          </CardContent>
        </Card>
      </div>
    );
  }

  if (isError || !student) {
    return (
      <div className="min-h-screen w-full flex items-center justify-center p-4">
        <Card className="w-full sm:max-w-md">
          <CardContent className="pt-6 text-center space-y-4">
            <p className="text-sm text-muted-foreground">
              We couldn't load this student. They may have been removed.
            </p>
            <Button variant="outline" onClick={() => navigate("/students")}>
              Back to Students
            </Button>
          </CardContent>
        </Card>
      </div>
    );
  }

  const dob = student.dateOfBirth
    ? new Date(student.dateOfBirth).toLocaleDateString(undefined, {
        year: "numeric",
        month: "long",
        day: "numeric",
      })
    : "";

  return (
    <div className="min-h-screen w-full flex items-center justify-center p-4">
      <Card className="w-full sm:max-w-xl">
        <CardHeader>
          <div className="flex items-center justify-between gap-4">
            <div className="flex items-center gap-2 min-w-0">
              <User className="size-5 text-muted-foreground shrink-0" />
              <CardTitle className="text-2xl font-bold tracking-tight truncate">
                {student.name}
              </CardTitle>
            </div>
            {isAdmin && (
              <Button
                variant="outline"
                size="sm"
                onClick={() => navigate(`/students/${id}/edit`)}
              >
                <Pencil className="size-4" />
                Edit
              </Button>
            )}
          </div>
        </CardHeader>

        <CardContent className="space-y-6">
          <div className="grid grid-cols-2 gap-4">
            <DetailItem icon={Mail} label="Email" value={student.email} />
            <DetailItem
              icon={Calendar}
              label="Age"
              value={String(student.age)}
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <DetailItem icon={MapPin} label="Address" value={student.address} />
            <DetailItem icon={Calendar} label="Date of Birth" value={dob} />
          </div>

          {student.cvFileUrl && (
            <div className="mt-4 pt-4 border-t border-gray-100">
              <Link
                to={student.cvFileUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="inline-flex items-center gap-1.5 text-sm text-blue-600 hover:text-blue-700 font-medium"
              >
                <FileText className="w-4 h-4" /> View CV
              </Link>
            </div>
          )}

          <Separator />

          <div className="space-y-1">
            <div className="flex items-center gap-1.5 text-sm font-medium text-muted-foreground">
              <FileText className="size-4" />
              Description
            </div>
            <p className="text-sm leading-relaxed">{student.description}</p>
          </div>

          <Separator />

          <div className="space-y-2">
            <div className="flex items-center gap-1.5 text-sm font-medium text-muted-foreground">
              <GraduationCap className="size-4" />
              Education
            </div>

            {(!student.education || student.education.length === 0) && (
              <p className="text-sm text-muted-foreground border border-dashed rounded-lg py-4 text-center">
                No education on record.
              </p>
            )}

            <div className="space-y-2">
              {student.education?.map((entry, index) => (
                <div
                  key={index}
                  className="flex items-center justify-between border rounded-lg p-3 text-sm"
                >
                  <div>
                    <p className="font-medium">{entry.degree}</p>
                    <p className="text-muted-foreground">
                      {entry.institutionName}
                    </p>
                  </div>
                  <span className="text-muted-foreground tabular-nums">
                    {entry.year}
                  </span>
                </div>
              ))}
            </div>
          </div>
        </CardContent>

        {/* Added Card Footer for See All List Button at the bottom */}
        <CardFooter className="flex justify-start pt-4 border-t">
          <Button
            variant="outline"
            className="gap-2"
            onClick={() => navigate("/students")}
          >
            <ArrowLeft className="size-4" />
            See All Students
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
}
