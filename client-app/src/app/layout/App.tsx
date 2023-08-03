import React, {useEffect} from "react";
import {Container} from "semantic-ui-react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import LoadingComponent from "./LoadingComponent";
import {useStore} from "../stores/store";
import {observer} from "mobx-react-lite";

function App() {
    const {activityStore} = useStore();

    useEffect(() => {
        activityStore.loadActivities().then(_ => {});
    }, [activityStore]);

    if (activityStore.loadingInitial) return <LoadingComponent inverted={true} content={"Loading activities..."}/>;

    return (
        <>
            <NavBar/>
            <Container style={{marginTop: "7em"}}>
                <ActivityDashboard />
            </Container>
        </>
    );
}

export default observer(App);
