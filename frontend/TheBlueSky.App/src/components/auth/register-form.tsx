import { useState } from "react"
import { toast } from "sonner"

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"

import AuthService from "@/services/AuthService"
import { ACCESS_KEY, REFRESH_KEY } from "@/config"

const isEmail = (e: string) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(e.trim())

const passwordRules = [
  { test: (s: string) => s.length >= 6, label: "At least 6 characters" },
  { test: (s: string) => /[a-z]/.test(s), label: "1 lowercase" },
  { test: (s: string) => /[A-Z]/.test(s), label: "1 uppercase" },
  { test: (s: string) => /\d/.test(s), label: "1 digit" },
  { test: (s: string) => /[^a-zA-Z0-9]/.test(s), label: "1 symbol" },
]

export function RegisterForm({ 
    className,
    ...props
}: React.ComponentProps<"div">) {
  const [firstName, setFirstName] = useState("")
  const [lastName, setLastName] = useState("")
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault()

    if (!firstName.trim()) return toast.error("Enter your first name")
    if (!lastName.trim()) return toast.error("Enter your last name")

    if (!isEmail(email)) return toast.error("Enter a valid email")

    if (!passwordRules.every((r) => r.test(password))) {
      return toast.error("Password does not meet requirements")
    }

    if (password !== confirmPassword) return toast.error("Passwords do not match")

    setLoading(true)
    try {

      const reg = await AuthService.register({ 
        firstName, 
        lastName, 
        email, 
        password 
      })

      if (!reg?.success) {
        toast.error(reg?.message || "Registration failed. Please try again.")
        return
      }

      toast.success("Account created. Signing you in...")

      const loginRes = await AuthService.login({ email, password })
      if (!loginRes?.success) {
        toast.error(loginRes?.message || "Login failed. Please sign in manually.")
        return
      }

      if (loginRes.accessToken) localStorage.setItem(ACCESS_KEY, loginRes.accessToken)
      if (loginRes.refreshToken) localStorage.setItem(REFRESH_KEY, loginRes.refreshToken)

      toast.success("Logged in successfully")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Create your account</CardTitle>
          <CardDescription>Sign up with your email</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <div className="grid gap-6">
              <div className="grid gap-3">
                <Label htmlFor="firstName">First name</Label>
                <Input 
                id="firstName" value={firstName} onChange={(e) => setFirstName(e.target.value)} required />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="lastName">Last name</Label>
                <Input id="lastName" value={lastName} onChange={(e) => setLastName(e.target.value)} required />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="email">Email</Label>
                <Input id="email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="password">Password</Label>
                <Input
                  id="password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
                
                <ul className="text-xs space-y-1">
                  {passwordRules.map((r) => (
                    <li key={r.label} className={r.test(password) ? "text-green-600" : "text-muted-foreground"}>
                      {r.test(password) ? "✓" : "•"} {r.label}
                    </li>
                  ))}
                </ul>
              </div>
              <div className="grid gap-3">
                <Label htmlFor="confirmPassword">Confirm password</Label>
                <Input
                  id="confirmPassword"
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  required
                />
              </div>
              <Button type="submit" className="w-full" disabled={loading}>
                {loading ? "Creating account..." : "Create account"}
              </Button>
              <div className="text-center text-sm">
                Already have an account?{" "}
                <a href="/login" className="underline underline-offset-4">Log in</a>
              </div>
            </div>
          </form>
        </CardContent>
      </Card>

      <div className="text-muted-foreground *:[a]:hover:text-primary text-center text-xs text-balance *:[a]:underline *:[a]:underline-offset-4">
        By signing up, you agree to our <a href="#">Terms of Service</a>{" "}
        and <a href="#">Privacy Policy</a>.
      </div>
    </div>
  )
}