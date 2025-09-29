export const isPriorityAvailable = (
    priority: number,
    usedPriorities: number[],
    originalPriority?: number
): boolean => {

    if (originalPriority !== undefined && priority === originalPriority) {
        return true; // allow unchanged in edit mode
    }
    return !usedPriorities.includes(priority);
};
