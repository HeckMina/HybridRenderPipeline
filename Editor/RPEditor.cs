using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Alice.Rendering
{
    public class RPEditor : EditorWindow
    {
        public List<Node> mNodes=new List<Node>();
        public List<Link> mLinks = new List<Link>();
        Vector2 mLastMouseButtonDownPosition;
        Vector2 mDragPosition,mDragOffsetToLocalOriginalPosition;
        Node mLastMiddleButtonTouchedNode = null;
        Node mEditNameNode = null;
        Node mCurrentSelectedNode = null;
        RTSetting mRTSettingInfo = new RTSetting();
        RPSetting mRPSettingInfo = new RPSetting(0,0,300,400);
        Link mTempLink=null;
        bool mbShowCreateOptions = false;
        public static RPEditor mInstance = null;
        public RPEditor()
        {
            mInstance = this;
        }
        public void SetPipelineData(Hybrid.PipelineData inPipelineData){

        }
        void OnGUI()
        {
            ProcessMouseEvent();
            //draw graphic node
            foreach (Node node in mNodes)
            {
                node.Draw();
            }
            if (mCurrentSelectedNode != null)
            {
                if (mCurrentSelectedNode.GetType() == typeof(RPNode))
                {
                    mRPSettingInfo.Draw(position.size, (RPNode)mCurrentSelectedNode);
                }else if(mCurrentSelectedNode.GetType() == typeof(RTNode))
                {
                    mRTSettingInfo.Draw(position.size, mCurrentSelectedNode as RTNode);
                }
               
            }
            if (mbShowCreateOptions)
            {
                ShowCreateOptions();
                mbShowCreateOptions = false;
            }
            //draw links
            foreach (Link link in mLinks)
            {
                link.Draw(position.size);
            }
            if (mTempLink != null)
            {
                mTempLink.Draw(position.size);
            }
        }
        void OnEditName(Node inNode)
        {
            mEditNameNode = inNode;
        }
        void OnCreateRenderPass(float inX, float inY)
        {
            RPNode rpNode = new RPNode(inX, inY, 120, 60);
            mNodes.Add(rpNode);
        }
        void OnCreateRenderTarget(float inX, float inY)
        {
            RTNode rpNode = new RTNode(inX, inY, 100, 40);
            mNodes.Add(rpNode);
        }
        void ProcessMouseEvent()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                mLastMouseButtonDownPosition = Event.current.mousePosition;
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonDown();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonDown();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonDown();
                }
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonUp();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonUp();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonUp();
                }
            }else if (Event.current.type == EventType.MouseDrag)
            {
                if (Event.current.button == 0)//left button
                {
                    OnMouseLeftButtonDrag();
                }
                else if (Event.current.button == 1)//right button
                {
                    OnMouseRightButtonDrag();
                }
                else if (Event.current.button == 2)//middle button
                {
                    OnMouseMiddleButtonDrag();
                }
            }
        }
        void OnMouseLeftButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    //Debug.Log("on left button down "+ node);
                    break;
                }
            }
            if (touchedNode != null)
            {
                if (Event.current.control)
                {
                    touchedNode.OnEditName();
                    mEditNameNode = touchedNode;
                }
                else if (Event.current.shift)
                {
                    if (touchedNode.IsClickOnTheLeftPart(Event.current.mousePosition.x))
                    {
                        if (touchedNode.mIncomingLink != null)
                        {
                            Link link = touchedNode.mIncomingLink;
                            Node other = GetNode(link.mStartRP);
                            other.mOutgoingLink = null;
                            touchedNode.mIncomingLink = null;
                            mLinks.Remove(link);
                        }
                    }
                    else
                    {
                        if (touchedNode.mOutgoingLink != null)
                        {
                            Link link = touchedNode.mOutgoingLink;
                            Node other = GetNode(link.mEndRP);
                            other.mIncomingLink = null;
                            touchedNode.mOutgoingLink = null;
                            mLinks.Remove(link);
                        }
                    }
                }
                else
                {
                    mCurrentSelectedNode = touchedNode;
                }
            }
        }
        void OnMouseMiddleButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (touchedNode != null)
            {
                mLastMiddleButtonTouchedNode = touchedNode;
                mDragPosition = mLastMouseButtonDownPosition;
                mDragOffsetToLocalOriginalPosition = mDragPosition - touchedNode.mRect.position;
            }
        }
        void OnMouseRightButtonDown()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (touchedNode == null)
            {
                mbShowCreateOptions = true;
            }
            else
            {

            }
        }
        void OnMouseLeftButtonUp()
        {
            Node touchedNode = null;
            foreach (Node node in mNodes)
            {
                if (node.mRect.Contains(Event.current.mousePosition))
                {
                    touchedNode = node;
                    break;
                }
            }
            if (mTempLink != null)
            {
                if (touchedNode != null)
                {
                    if (typeof(RPNode) == touchedNode.GetType()&&mTempLink.mStartRP!=touchedNode.mID)
                    {
                        mTempLink.mEndRP = touchedNode.mID;
                        touchedNode.mIncomingLink = mTempLink;
                        mTempLink.SetPoint(1, touchedNode.GetLeftConnectionPoint());
                        mLinks.Add(mTempLink);
                    }
                }
                else
                {
                    Node startNode = GetNode(mTempLink.mStartRP);
                    startNode.mOutgoingLink = null;
                }
                GUI.changed = true;
                mTempLink = null;
            }
            else
            {
                if (!mRPSettingInfo.mRect.Contains(Event.current.mousePosition)&&!mRTSettingInfo.mRect.Contains(Event.current.mousePosition))
                {
                    mCurrentSelectedNode = touchedNode;
                    GUI.changed = true;
                }
            }
            if (mEditNameNode != null)
            {
                if (!mEditNameNode.mRect.Contains(Event.current.mousePosition))
                {
                    mEditNameNode.OnFinishedEditName();
                    mEditNameNode = null;
                    GUI.changed = true;
                }
            }
        }
        void OnMouseMiddleButtonUp()
        {
            if (mLastMiddleButtonTouchedNode != null)
            {
                mLastMiddleButtonTouchedNode = null;
            }
        }
        void OnMouseRightButtonUp()
        {
        }
        void OnMouseLeftButtonDrag()
        {
            if (mCurrentSelectedNode != null && mEditNameNode==null)
            {
                if (mTempLink == null && mCurrentSelectedNode.mOutgoingLink == null && mCurrentSelectedNode.GetType()==typeof(RPNode))
                {
                    float x = mCurrentSelectedNode.mRect.xMax;
                    float y = mCurrentSelectedNode.mRect.yMin + mCurrentSelectedNode.mRect.height / 2;
                    mTempLink = new Link(new Vector3(x, y, 0.0f), new Vector3(x, y, 0.0f), new Vector3(x, y, 0.0f), new Vector3(x, y, 0.0f));
                    mTempLink.mStartRP = mCurrentSelectedNode.mID;
                    mCurrentSelectedNode.mOutgoingLink = mTempLink;
                }
                else
                {
                    if (mTempLink != null)
                    {
                        mTempLink.SetPoint(1, Event.current.mousePosition);
                        GUI.changed = true;
                    }
                }
            }
        }
        void OnMouseRightButtonDrag()
        {
        }
        void OnMouseMiddleButtonDrag()
        {
            if (mLastMiddleButtonTouchedNode != null)
            {
                ProcessDragNode();
            }
        }
        void ShowCreateOptions()
        {
            GUILayout.BeginArea(new Rect(10, 10, position.width - 20, position.height - 20));
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create Render Pass"), false, () => { OnCreateRenderPass(mLastMouseButtonDownPosition.x, mLastMouseButtonDownPosition.y); });
            menu.AddItem(new GUIContent("Create Render Target"), false, () => { OnCreateRenderTarget(mLastMouseButtonDownPosition.x, mLastMouseButtonDownPosition.y); });
            menu.ShowAsContext();
            GUILayout.EndArea();
        }
        void ProcessDragNode()
        {
            Vector2 draggingOffset = Event.current.mousePosition - mDragPosition;
            Vector2 newPosition = mDragPosition + draggingOffset - mDragOffsetToLocalOriginalPosition;
            mLastMiddleButtonTouchedNode.mRect.position = newPosition;
            mDragPosition = Event.current.mousePosition;
            if (mLastMiddleButtonTouchedNode.mOutgoingLink != null)
            {
                mLastMiddleButtonTouchedNode.mOutgoingLink.SetPoint(0, mLastMiddleButtonTouchedNode.GetRightConnectionPoint());
            }
            if (mLastMiddleButtonTouchedNode.mIncomingLink != null)
            {
                mLastMiddleButtonTouchedNode.mIncomingLink.SetPoint(1, mLastMiddleButtonTouchedNode.GetLeftConnectionPoint());
            }
            GUI.changed = true;
        }
        Node GetNode(string inID)
        {
            foreach(Node node in mNodes)
            {
                if (node.mID.CompareTo(inID)==0) {
                    return node;
                }
            }
            return null;
        }
    }
}
