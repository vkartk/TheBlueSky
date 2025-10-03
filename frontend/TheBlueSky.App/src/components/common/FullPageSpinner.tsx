"use client";

import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";
type FullPageSpinnerProps = {
  label?: string;
  className?: string;
  dim?: boolean;
};

export function FullPageSpinner({ label = "Loading…", className, dim = true }: FullPageSpinnerProps) {
  return (
    <div
      role="status"
      aria-live="polite"
      aria-busy="true"
      className={cn(
        "fixed inset-0 z-50 grid place-items-center",
        dim && "bg-background/70 backdrop-blur-sm",
        className
      )}
    >
      <div className="flex flex-col items-center gap-3">
        <Loader2 className="h-8 w-8 animate-spin" />
        <span className="text-sm text-muted-foreground">{label}</span>
      </div>
    </div>
  );
}
