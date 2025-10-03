'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useLocation, useNavigate } from 'react-router';
import { Loader2 } from 'lucide-react';

import { cn } from '@/lib/utils';
import { loginSchema, type LoginFormData } from '@/lib/validators/authSchemas';
import { useAppDispatch, useAppSelector } from '@/store';
import { loginUser } from '@/features/auth/authThunks';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';

export const LoginForm = ({ className, ...props }: React.ComponentProps<'div'>) => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const location = useLocation();
  
  const { loading } = useAppSelector((state) => state.auth);
  const isSubmitting = loading === 'pending';

  const form = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  const onSubmit = async (data: LoginFormData) => {
    const resultAction = await dispatch(loginUser(data));
    if (loginUser.fulfilled.match(resultAction)) {
      // Check for a saved path in the location's state, or default to the homepage.
      const fromPath = location.state?.from?.pathname || '/';
      navigate(fromPath, { replace: true });
    }
  };

  return (
    <div className={cn('flex flex-col gap-6', className)} {...props}>
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Welcome back</CardTitle>
          <CardDescription>Login to your account</CardDescription>
        </CardHeader>
        <CardContent>
          {/* The rest of your JSX remains the same */}
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
              {/* ... FormField for email */}
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Email</FormLabel>
                    <FormControl>
                      <Input placeholder="m@example.com" {...field} disabled={isSubmitting} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              {/* ... FormField for password */}
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <div className="flex items-center">
                      <FormLabel>Password</FormLabel>
                      <a href="#" className="ml-auto text-sm underline-offset-4 hover:underline">
                        Forgot your password?
                      </a>
                    </div>
                    <FormControl>
                      <Input
                        type="password"
                        placeholder="Your password"
                        {...field}
                        disabled={isSubmitting}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button type="submit" className="w-full" disabled={isSubmitting}>
                {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                {isSubmitting ? 'Logging in...' : 'Login'}
              </Button>
            </form>
          </Form>
          <div className="mt-6 text-center text-sm">
            Don&apos;t have an account?{' '}
            <a href="/register" className="underline underline-offset-4">
              Sign up
            </a>
          </div>
        </CardContent>
      </Card>
    </div>
  );
};