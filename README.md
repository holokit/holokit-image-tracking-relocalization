# Image Tracking Relocalization

In certain AR projects, it's essential to transform the entire virtual coordinate system to align virtual content with the physical environment or synchronize multiple devices in multiplayer settings. This Unity plugin enhances ARFoundation's image tracking, enabling device coordinate system relocalization through tracked image poses. A notable challenge with ARFoundation's image tracking is its inconsistent stability; relying on single instances often leads to significant deviations. Our plugin addresses this by stablizing image tracking through a sequence of consecutively tracked image poses, improving the accuracy of the final outcome.

The plugin provides two relocalization methods, which are external marker-based and dynamically rendered marker-based approaches, making it suitable for both single-player to multiplayer experiences.

## External Marker Relocalization

This approach is suited for both single-player and multiplayer projects. It functions by utilizing an external marker image, which, when scanned by one or several devices, allows them to relocalize their coordinate origins to the image's location. This method is straightforward and easy to implement. However, it's important to note that external markers might not always be available or suitable for every scenario.

To facilitate a smooth start, we've included an "External Marker Relocalization" sample in the package. Importing this sample into your project can help you quickly grasp the setup and usage of this feature.

### How To Use External Marker To Relocalize

For relocalizing the coordinate system with an external marker, the `ImageTrackingStablizer` and `WorldTransformResetter` scripts are required.

<img width="368" alt="image" src="https://github.com/holoi/com.holoi.xr.image-tracking-relocalization/assets/44870300/09892e95-64af-4015-a5d1-28fbaeb1ec8f">

The `ImageTrackingStablizer` script calculates the standard deviation of poses from multiple consecutively tracked images. When this deviation falls below a predefined threshold, indicating stable tracking, it captures the pose of the last tracked image as the final pose. Subsequently, it triggers a Unity event `OnTrackedImagePoseStablized`, passing the image's position and rotation as parameters.

The `OnTrackedImagePoseStablized` Unity event is received by the `WorldTransformResetter` script. To facilitate easy transformation of the entire coordinate system, all game objects are nested under a root object named "World Transform". Upon detecting a stable image pose, this root object is moved to the image location, causing all child objects to follow suit. While alternative methods for coordinate system relocalization are possible, this straightforward approach offers a viable option.

<img width="423" alt="image" src="https://github.com/holoi/com.holoi.xr.image-tracking-relocalization/assets/44870300/e257d028-e2d9-4953-986e-844271fef596">

If you choose not to use a root object for relocalizing all game objects, you can omit the `WorldTransformResetter` script and apply your own method. Simply handle the `OnTrackedImagePoseStablized` event from the `ImageTrackingStablizer` in your custom script.

The accompanying GIF demonstrates the external marker relocalization process. Initially, a cube marked with x, y, and z axes is positioned at (0, 0, 0). Once the image tracking algorithm identifies a stable pose of the QR code, the coordinate system relocates to the image's position, resulting in the cube moving to the image location.

![ezgif-1-d80370e909](https://github.com/holoi/com.holoi.xr.image-tracking-relocalization/assets/44870300/ba8d577c-5b87-4244-a5a5-dce3cffe7c94)

## Dynamically Renderer Marker Relocalization

In many cases, an external marker might not be readily available, as it's impractical to expect users to carry such markers with them at all times. Addressing this, our plugin offers a solution specifically designed for multiplayer AR experiences. In this setup, the host device dynamically renders a marker image on its screen, which client devices then scan to relocalize their coordinate systems. This method, while more sophisticated due to the involvement of network communications, offers a practical solution for multiplayer scenarios where external markers are not feasible.

### How Dynamically Rendered Marker Relocalization Works

This approach focuses on stable tracking of a marker displayed on the host device's screen. It synchronizes the coordinate origins of all client devices with the host device's origin, utilizing a series of tracked image poses and communication between the host and client devices.

The synchronization process comprises two phases. The first phase calculate the timestamp offset between the host and client devices. In the second phase, the relative translation and rotation from the client to the host device are determined.

#### Timestamp Synchronization

The goal of this phase is to determine the timestamp offset between a client and the host devices. This is crucial for aligning each image pose tracked by the client device with its corresponding pose in the host's coordinate system.

In this phase, the client device continuously sends its current local timestamp to the host device. Upon receiving a timestamp from a client, the host replies with its local timestamp at that time. The client then calculates an instance of the timestamp offset using the formula:

```
timestampOffset = hostTimestamp + (currentClientTimestamp - previousClientTimestamp) / 2 - currentClientTimestamp
```

The client accumulates these calculated offsets in a queue. Once the queue size surpasses a set threshold, the client computes the standard deviation of the offsets. If this deviation is below a predefined value, indicating stability, the client then averages all queue elements to determine the final timestamp offset.

#### Coordinate System Synchronization

After calculating the local timestamp offset relative to the host, a client begins tracking the target marker displayed on the host device's screen. Each time the client detects a tracked image pose, it requests the host to provide the corresponding pose with the nearest timestamp. This forms a pair of image poses: one from the client's local coordinate system and the corresponding one from the host's system. These pairs are queued on the client device.

When the queue size of these image pose pairs exceeds a certain threshold, the least squares method is employed to compute the synchronization result. This result includes the relative translation and rotation from the client's origin to the host's origin. The client continually calculates and queues these synchronization resultss. Upon reaching another predefined threshold in the synchronization result queue, the standard deviation of the queue elements is calculated. If this deviation is sufficiently small, indicating stability, the latest element is taken as the final synchronization result. This final result is then used to reset the client device's coordinate system origin.

### How To Use Dynamically Rendered Marker To Relocalize

The package includes a sample named "Dynimically Rendered Marker Relocalization", which serves as an ideal starting point for your project. This sample requires Unity Netcode's [MultipeerConnectivity transport package](https://github.com/Unity-Technologies/multiplayer-community-contributions/tree/main/Transports/com.community.netcode.transport.multipeer-connectivity), which must be installed seperately. For installation instructions, please consult with the README file of the MultipeerConnectivity transport package. Note that MultipeerConnectivity, being Apple's networking framework, restricts this sample's compatibility to iOS devices only.




