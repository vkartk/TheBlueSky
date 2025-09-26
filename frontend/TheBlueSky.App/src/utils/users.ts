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