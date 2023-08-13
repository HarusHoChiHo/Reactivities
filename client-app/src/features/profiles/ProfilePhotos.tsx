import React, {SyntheticEvent, useState} from "react";
import {observer} from "mobx-react-lite";
import {Button, Card, Grid, Header, Image, Tab} from "semantic-ui-react";
import {Photo, Profile} from "../../app/models/profile";
import {useStore} from "../../app/stores/store";
import PhotoUploadWidget from "../../app/common/ImageUpload/PhotoUploadWidget";

interface Props {
    profile: Profile;
}

function ProfilePhotos({profile}: Props) {
    const {profileStore: {isCurrentUser, uploadPhoto, uploading, loading, setMainPhoto, deletePhoto}} = useStore();
    const [addPhotoMode, setAddPhotoMode] = useState(false);
    const [target, setTarget] = useState("");

    function handlePhotoUpload(file: Blob) {
        uploadPhoto(file).then(() => setAddPhotoMode(false));
    }

    function handleSetMainPhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        setMainPhoto(photo).then(_ => {});
    }

    function handleDeletePhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>) {
        setTarget(e.currentTarget.name);
        deletePhoto(photo);
    }

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header icon={"image"} content={"Photos"}/>
                    {isCurrentUser && (
                        <Button
                            floated={"right"}
                            basic={true}
                            content={addPhotoMode ? "Cancel" : "Add Photo"}
                            onClick={() => setAddPhotoMode(!addPhotoMode)}
                        />
                    )}
                </Grid.Column>
                <Grid.Column width={16}>
                    {addPhotoMode ? (
                        <PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={uploading}/>
                    ) : (<Card.Group itemsPerRow={5}>
                        {profile.photos?.map(photo => (
                            <Card>
                                <Image src={photo.url} key={photo.id}/>
                                {isCurrentUser && (
                                    <Button.Group fluid={true} widths={2}>
                                        <Button
                                            basic
                                            color={"green"}
                                            content={"Main"}
                                            name={"main" + photo.id}
                                            disabled={photo.isMain}
                                            loading={target === "main" + photo.id && loading}
                                            onClick={e => handleSetMainPhoto(photo, e)}
                                        />
                                        <Button
                                            basic
                                            color={"red"}
                                            icon={"trash"}
                                            loading={target === photo.id && loading}
                                            onClick={e => handleDeletePhoto(photo, e)}
                                            disabled={photo.isMain}
                                            name={photo.id}
                                        />
                                    </Button.Group>
                                )}
                            </Card>
                        ))}
                    </Card.Group>)
                    }
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
}

export default observer(ProfilePhotos);