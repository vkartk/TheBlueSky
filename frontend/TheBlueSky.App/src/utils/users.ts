import type { User } from "@/types/auth"

export const getNameInitials = (name: string | null | undefined): string => {
    if (!name) return "GU"

    return name
        .split(" ")
        .map(word => word[0])
        .filter(Boolean)
        .join("")
        .toUpperCase()
        .slice(0, 2)
}

export const getUserAvatar = (user:User) => {
    const text = getNameInitials(user.firstName+ ' '+ user.lastName);
    return `https://avatar.vercel.sh/${encodeURIComponent(user.userId)}.svg?text=${encodeURIComponent(text)}`;

}