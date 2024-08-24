# Import necessary libraries
import cv2  # For image and video processing
from cvzone.HandTrackingModule import HandDetector  # Hand tracking module from cvzone
import socket  # For network communication

# Set the width and height for the video capture
width, height = 1200, 720

# Initialize video capture with the default camera
cap = cv2.VideoCapture(0)
# Set the width of the video frames
cap.set(3, width)
# Set the height of the video frames
cap.set(4, height)

# Initialize the hand detector
# maxHands=2: Detects up to 2 hands
# detectionCon=0.85: Sets the minimum detection confidence to 80%
detector = HandDetector(maxHands=2, detectionCon=0.5)

# Setup the UDP socket for sending data
unity_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
# Define the address and port to send data to (in this case, localhost and port 12345)
unity_address = ("127.0.0.1", 12345)

# Start an infinite loop to process video frames
while True:
    # Read a frame from the video capture
    success, img = cap.read()
    # If the frame was not successfully read (e.g., end of video), print an error and break the loop
    if not success:
        print("Failed to grab frame")
        break

    # Apply a horizontal flip (mirror the image)
    img = cv2.flip(img, 1)

    # Find hands in the current frame
    # The returned 'hands' variable is a list of detected hands with their details
    hands, img = detector.findHands(img)

    # Initialize a list to store data about hand landmarks
    data = []

    # If any hands are detected, process them
    if hands:
        # Iterate through each detected hand
        for hand in hands:
            # Get the list of landmark positions for the current hand
            lmList = hand["lmList"]

            # For each landmark in the current hand
            for lm in lmList:
                # Add the x, y, z coordinates to the data list, adjusting the y-coordinate to match the image's coordinate system
                data.extend([lm[0], height - lm[1], lm[2]])

        # If there is any data collected from the landmarks, send it to Unity
        if data:
            # Convert the data list to a string and encode it to bytes, then send it via UDP
            unity_socket.sendto(str.encode(str(data)), unity_address)

    # Resize the displayed image to half size for better performance and display it in a window titled "Image"
    img = cv2.resize(img, (0, 0), None, 0.5, 0.5)
    cv2.imshow("Image", img)

    # Check if the 'q' key is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        # If 'q' is pressed, break the loop and end the program
        break

# After breaking out of the loop, release the video capture object and close all OpenCV windows
cap.release()
cv2.destroyAllWindows()
