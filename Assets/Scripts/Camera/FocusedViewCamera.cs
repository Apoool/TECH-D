﻿using UnityEngine;
using Cinemachine;
using System;

public class FocusedViewCamera : MonoBehaviour {

    [Header("Non-FocusedViewTarget tag")]
    // this means regular lecture services/rooms
    public Vector3 adjustedFollowOffset = new Vector3(-10, 0, 0);
    public Vector3 adjustedTrackedObjectOffset = Vector3.zero;

    [Header("With AgentFollowTarget tag")]
    public Vector3 adjustedAgentFollowOffset = new Vector3(-10, 0, 0);
    public Vector3 adjustedAgentTrackedObjectOffset = Vector3.zero;

    [Header("Original Follow Offset")]
    public Vector3 baseFollowOffset;
    public Vector3 baseTrackedObjectOffset;

    private CinemachineVirtualCamera focusedViewCamera;
    private Transform currentTarget;
    public bool allowOffset = true;

    private void Awake() {
        focusedViewCamera = GetComponent<CinemachineVirtualCamera>();
        baseFollowOffset =  focusedViewCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        baseTrackedObjectOffset =  focusedViewCamera.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset;
    }

    private void Update() {
        currentTarget = focusedViewCamera.m_Follow;

        if (allowOffset) {
            HandleFocusedOffset();
        }
    }

    private void HandleFocusedOffset() {

        if (currentTarget != null && currentTarget.CompareTag("AgentFollowTarget")) {
            AdjustAimOffset(focusedViewCamera, adjustedAgentFollowOffset);
            AdjustTrackedOffset(focusedViewCamera, adjustedAgentTrackedObjectOffset);
        }
        else if (currentTarget != null && !currentTarget.CompareTag("FocusedViewTarget")) {
            AdjustAimOffset(focusedViewCamera, adjustedFollowOffset);
            AdjustTrackedOffset(focusedViewCamera, adjustedTrackedObjectOffset);
        } 
        else {
            RestoreAimOffset(focusedViewCamera, baseFollowOffset);
            RestoreTrackedOffset(focusedViewCamera, baseTrackedObjectOffset);
        }
    }

    private void AdjustAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = offset;
    }

    private void RestoreAimOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camTransposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        camTransposer.m_FollowOffset = baseFollowOffset;
    }

    private void AdjustTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = offset;
    }

    private void RestoreTrackedOffset(CinemachineVirtualCamera camera, Vector3 offset) {
        var camComposer = camera.GetCinemachineComponent<CinemachineComposer>();
        camComposer.m_TrackedObjectOffset = baseTrackedObjectOffset;
    }
}