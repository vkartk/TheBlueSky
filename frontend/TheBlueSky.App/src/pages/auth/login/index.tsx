import { Plane } from "lucide-react"
import { Link } from "react-router"
import { LoginForm } from "@/components/auth/login-form"

export default function LoginPage() {
    return (
        <div className="bg-muted flex min-h-svh flex-col items-center justify-center gap-6 p-6 md:p-10">
            <div className="flex w-full max-w-sm flex-col gap-6">
                <Link
                    to="/"
                    className="flex items-center gap-2 font-bold justify-center"
                    aria-label="TheBlueSky home"
                >
                    <Plane className="h-12 w-12 text-blue-500" />
                    <span className="text-3xl">
                        The<span className="text-blue-500">BlueSky</span>
                    </span>
                </Link>
                <LoginForm />
            </div>
        </div>
    )
}