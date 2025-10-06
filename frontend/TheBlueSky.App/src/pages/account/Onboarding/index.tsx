import { useNavigate } from 'react-router';
import { useAppSelector } from '@/store';
import { ProfileSetupForm } from '@/components/pages/OnboardingPage/ProfileSetupForm';

const OnboardingPage = () => {
    const navigate = useNavigate();

    const authUser = useAppSelector(state => state.auth.user);

    if (!authUser) return;

    return (
        <ProfileSetupForm
            userId={authUser.userId}
            userEmail={authUser.email}
            firstName={authUser.firstName}
            lastName={authUser.lastName}
            onComplete={() =>
                navigate('/')
            }
        />
    );
};

export default OnboardingPage;
