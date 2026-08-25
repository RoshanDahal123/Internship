interface DetailItemProps {
  icon: React.ElementType
  label: string
  value: string
}

export default function DetailItem({ icon: Icon, label, value }: DetailItemProps) {
  return (
    <div className="space-y-1">
      <div className="flex items-center gap-1.5 text-sm font-medium text-muted-foreground">
        <Icon className="size-4" />
        {label}
      </div>
      <p className="text-sm">{value || "—"}</p>
    </div>
  )
}
